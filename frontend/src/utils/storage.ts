const HAS_FINISHED_DAILY_KEY = "hasFinishedDaily";
const LAST_RESET_DATE_KEY = "lastResetDate";

export const storage = {
  // Check if it's a new day
  isNewDay(): boolean {
    const lastReset = localStorage.getItem(LAST_RESET_DATE_KEY);
    if (!lastReset) return true;

    const lastResetDate = new Date(lastReset);
    const today = new Date();

    return (
      lastResetDate.getDate() !== today.getDate() ||
      lastResetDate.getMonth() !== today.getMonth() ||
      lastResetDate.getFullYear() !== today.getFullYear()
    );
  },

  // Reset daily progress
  resetDailyProgress(): void {
    localStorage.setItem(HAS_FINISHED_DAILY_KEY, "false");
    localStorage.setItem(LAST_RESET_DATE_KEY, new Date().toISOString());
  },

  // Get hasFinishedDaily status
  getHasFinishedDaily(): boolean {
    // Check if it's a new day first
    if (this.isNewDay()) {
      this.resetDailyProgress();
      return false;
    }

    const value = localStorage.getItem(HAS_FINISHED_DAILY_KEY);
    return value === "true";
  },

  // Set hasFinishedDaily status
  setHasFinishedDaily(value: boolean): void {
    localStorage.setItem(HAS_FINISHED_DAILY_KEY, String(value));

    // Set last reset date if not exists
    if (!localStorage.getItem(LAST_RESET_DATE_KEY)) {
      localStorage.setItem(LAST_RESET_DATE_KEY, new Date().toISOString());
    }
  },

  // Initialize storage on app start
  initialize(): void {
    if (this.isNewDay()) {
      this.resetDailyProgress();
    } else if (!localStorage.getItem(HAS_FINISHED_DAILY_KEY)) {
      this.setHasFinishedDaily(false);
    }
  },

  // Get time until midnight
  getTimeToMidnight(): { hours: number; minutes: number; seconds: number } {
    const now = new Date();
    const midnight = new Date(now);
    midnight.setHours(24, 0, 0, 0);

    const diff = midnight.getTime() - now.getTime();

    const hours = Math.floor(diff / (1000 * 60 * 60));
    const minutes = Math.floor((diff / (1000 * 60)) % 60);
    const seconds = Math.floor((diff / 1000) % 60);

    return { hours, minutes, seconds };
  },
};
