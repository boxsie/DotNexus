import $ from 'jquery';
import Vue from 'vue';
import 'bootstrap';
import { ipcRenderer } from 'electron';

import '../Styles/layout.scss';

export default class Layout {
    constructor() {
        window.vm = new Vue({
            el: '#layout',
            data: {
            },
            mounted() {
                ipcRenderer.send('notification', 'ping');
            }
        });
    }
}