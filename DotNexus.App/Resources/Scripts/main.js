import $ from 'jquery';
import Vue from 'vue';
import 'bootstrap';
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

import layout from './layout';

import '../Styles/layout.scss';
import '../Images/logo.png';

export class Main {
    constructor(options) {
        Vue.use(layout, options);

        window.vm = new Vue({
            el: '#header',
            mounted() {
            }
        });
    }
}