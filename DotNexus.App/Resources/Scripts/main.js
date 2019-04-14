import $ from 'jquery';
import Vue from 'vue';
import 'bootstrap';

import '../Styles/layout.scss';
import '../Images/logo.png';

export default class Layout {
    constructor() {
        window.vm = new Vue({
            el: '#layout',
            data: {
            },
            mounted() {
            }
        });
    }
}