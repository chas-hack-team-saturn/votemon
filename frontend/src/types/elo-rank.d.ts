declare module 'elo-rank' {
  class EloRank {
    constructor(k?: number);
    setKFactor(k: number): void;
    getKFactor(): number;
    getExpected(a: number, b: number): number;
    updateRating(expected: number, actual: number, current: number): number;
  }
  export default EloRank;
}
