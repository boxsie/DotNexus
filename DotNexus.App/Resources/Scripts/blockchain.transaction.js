import $ from 'jquery';
import Vue from 'vue';
import Avatars from '@dicebear/avatars';
import SpriteCollection from '@dicebear/avatars-identicon-sprites';

export class BlockchainTransaction {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                identiconSvg: ''
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