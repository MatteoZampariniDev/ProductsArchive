export function isNullOrWhiteSpaces(s: string): boolean {
  return s ? s.trim().length < 1 : false;
}
