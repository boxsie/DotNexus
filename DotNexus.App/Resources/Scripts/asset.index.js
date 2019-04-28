import $ from 'jquery';
import Vue from 'vue';
import Moment from 'moment';

import dataTableVue from './Library/dataTableVue';

export class AssetIndex {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                dtOptions: {
                    localData: options.assets
                }
            },
            components: {
                assetTable: dataTableVue
            },
            methods: {
                formatDate(date) {
                    return Moment(date).format('MMM DD YYYY');
                }
            },
            mounted() {
            }
        });
    }
}