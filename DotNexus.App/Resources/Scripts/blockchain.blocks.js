import $ from 'jquery';
import Vue from 'vue';

import dataTableVue from './Library/dataTableVue';

export class BlockchainBlocks {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                dtOptions: {
                    localData: options.latestBlocks.reverse()
                }
            },
            components: {
                blockTable: dataTableVue
            },
            mounted() {
                this.$layout.$on('new-block-notify', (block) => {
                    this.latestBlocks.splice(0, 0, block);
                    this.latestBlocks.pop();
                });
            }
        });
    }
}