export type ThemeType = 'light' | 'dark';

export interface ThemeState {
    ActiveTheme?: ThemeType;
    DarkMode?: boolean;
}