import $ from 'jquery';
import Vue from 'vue';
import dataTableVue from './Library/dataTableVue'; 

import '../Styles/manage.transactions.scss';

export class ManageTransactions {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                dtOptions: {
                    localData: options.txs
                }
            },
            components: {
                txTable: dataTableVue
            }
        });
    }
}