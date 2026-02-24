# ELO Backend Integration Guide

This document describes the changes needed in the `BackendAPI` to support persistent ELO ratings for Pokemon.

## 1. Update Database Schema

Modernize the `Pokemon` table to include an `EloRating` column.

### MariaDB SQL
```sql
ALTER TABLE Pokemon ADD COLUMN EloRating INT NOT NULL DEFAULT 1200;
```

## 2. Update Model

Modify `BackendAPI/Models/Pokemon.cs` to include the new property.

```csharp
public partial class Pokemon
{
    public int DexId { get; set; }
    public string Name { get; set; } = null!;
    public int? Votes { get; set; }
    public int EloRating { get; set; } // Add this
}
```

## 3. Update DbContext

Update `BackendAPI/Data/PokeScrandleDbContext.cs` to configure the new property.

```csharp
entity.Property(e => e.EloRating)
    .HasDefaultValueSql("'1200'")
    .HasColumnType("int(11)");
```

## 4. Update Voting Logic

Update the voting endpoint in `Program.cs` to calculate and save ELO ratings. Since the frontend now calculates the ELO, you can either trust the frontend (not recommended for production) or implement the ELO logic server-side.

### Recommended: Server-Side Calculation

The endpoint should be changed to accept both the winner and loser IDs.

```csharp
app.MapPut("/battle", async (PokeScrandleDbContext dB, int winnerId, int loserId) =>
{
    var winner = await dB.Pokemons.FindAsync(winnerId);
    var loser = await dB.Pokemons.FindAsync(loserId);

    if (winner == null || loser == null) return Results.NotFound();

    // ELO Logic
    double expectedWinner = 1.0 / (1.0 + Math.Pow(10, (loser.EloRating - winner.EloRating) / 400.0));
    double expectedLoser = 1.0 / (1.0 + Math.Pow(10, (winner.EloRating - loser.EloRating) / 400.0));

    int kFactor = 32;
    winner.EloRating = (int)Math.Round(winner.EloRating + kFactor * (1 - expectedWinner));
    loser.EloRating = (int)Math.Round(loser.EloRating + kFactor * (0 - expectedLoser));
    
    winner.Votes++;

    await dB.SaveChangesAsync();
    return Results.Ok();
});
```

## 5. Frontend Alignment

Once the backend is updated, the frontend `Battle.tsx` should be modified to call the new `/battle` endpoint instead of the simple `/vote` endpoint.
