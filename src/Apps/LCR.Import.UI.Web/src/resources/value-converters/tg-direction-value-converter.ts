export class TgDirectionValueConverter {
  toView(value) {
    if (!value) {
      return value;
    }

    switch (value) {
      case "3":
        return "I/O";
      case "2":
        return "O";
      case "1":
        return "I";
      default:
        return value;
    }
  }
}
