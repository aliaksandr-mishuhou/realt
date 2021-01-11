import { Item } from './item';
import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
export class StatsService {

    public getDaily() : Item[] {
        let data : Item[] = [
            {day: '2020-12-15', total: 150},
            {day: '2020-12-16', total: 120},
            {day: '2020-12-17', total: 125},
            {day: '2020-12-18', total: 133},
            {day: '2020-12-19', total: 112},
            {day: '2020-12-20', total: 112},
            {day: '2020-12-21', total: 112},
            {day: '2020-12-22', total: 140},
            {day: '2020-12-23', total: 121},
            {day: '2020-12-24', total: 122},
        ];
        return data;
    }
}