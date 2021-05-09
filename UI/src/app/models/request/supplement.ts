export interface Supplement {
  supplementId: string;
  supplementName: string;
  expirationDate: Date;
  certificatePath: string | ArrayBuffer;
}
