export class DetermineErrorsValueConverter {
  private errorCodes = [2, 4, 8, 16, 32];
  toView(value: number) {
    const res = this.errorCodes
      .map(c => value & c)
      .filter(c => c !== 0)
      ;
    return res;
  }
}

export class FormatFlagLabelValueConverter {
  private vocab: Map<number, string> = new Map([
    [2, "Номер строки"],
    [4, "Имя оператора"],
    [8, "Тип направления"],
    [16, "Дата открытия"],
    [32, "Дата закрытия"]
  ]);

  toView(value: number) {
    return this.vocab.get(value);
  }
}

export class FormatFlagDescriptionValueConverter {
  private vocab: Map<number, string> = new Map([
    [2, "Номер строки не указан"],
    [4, "Имя оператора или полное имя оператора сопряженного коммутатора не указаны"],
    [8, "Тип направления не указан или имеет недопустимое значение"],
    [16, "Дата открытия не указана или имеет недопустимое значение"],
    [32, "Дата закрытия не указана или имеет недопустимое значение"]
  ]);

  toView(value: number) {
    return this.vocab.get(value);
  }
}
