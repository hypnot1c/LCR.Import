import moment from 'moment';

export class DateTimeValueConverter {
    toView(value, format) {
        return moment(value).format(format);
    }
}
