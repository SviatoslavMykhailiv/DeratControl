import { Point } from '../point';

export interface Perimeter {
  perimeterId: string;
  facilityId: string;
  perimeterName: string;
  schemeImage: string;
  leftLoc: number;
  topLoc: number;
  points: Point[];
}
