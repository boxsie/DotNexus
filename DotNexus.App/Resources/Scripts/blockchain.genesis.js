import $ from 'jquery';
import Vue from 'vue';
import Avatars from '@dicebear/avatars';
import SpriteCollection from '@dicebear/avatars-identicon-sprites';
import dataTableVue from './Library/dataTableVue';

export class BlockchainGenesis {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                identiconSvg: '',
                dtOptions: {
                    localData: options.txs
                }
            },
            components: {
                txTable: dataTableVue
            },
            mounted() {
                if (options.genesis) {
                    const avatars = new Avatars(SpriteCollection);
                    const svg = avatars.create(options.genesis);
                    this.identiconSvg = svg;
                }
            }
        });
    }
}