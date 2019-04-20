import $ from 'jquery';
import Vue from 'vue';
import Avatars from '@dicebear/avatars';
import SpriteCollection from '@dicebear/avatars-identicon-sprites';

import '../Styles/blockchain.block.scss';

export class BlockchainBlock {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
            },
            methods: {
                identiconSvg(hash) {
                    const avatars = new Avatars(SpriteCollection);
                    return avatars.create(hash);
                }
            }
        });
    }
}