export interface Incident {
  id: string;
  category: string;
  severity: string | null;
  areaRadius: number;
  timestamp: string;
  longitude: number;
  latitude: number;
  userToReport: string;
  status: string;
  description: string | null;
}