export class ModelStateErrors {
  errors: ModelStateError[] = [];
}

export class ModelStateError {
  property: string = "";
  descriptions: string[] = [];
}
