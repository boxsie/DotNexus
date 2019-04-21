import $ from 'jquery';
import Vue from 'vue';
import 'bootstrap';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

import layout from './layout';

export * from './blockchain.block';
export * from './blockchain.blocks';
export * from './blockchain.transaction';
export * from './blockchain.genesis';

import '../Styles/layout.scss';
import '../Images/logo.png';

export class Main {
    constructor(options) {
        Vue.use(layout, options);
    }
}