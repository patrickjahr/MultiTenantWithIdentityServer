import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
    name: 'customDate',
})
export class CustomDatePipe implements PipeTransform {

    transform(value: number, args?: string): string {
        if (value) {
            if (!args) {
                const dateString = moment.unix(value).format('YYYY-MM-DD');
                return dateString;
            }
            return moment(value).format(args);
        }
        return '-';
    }
}
