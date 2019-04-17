import $ from 'jquery';
import Vue from 'vue';

export class BlockchainBlocks {
    constructor(options) {
        window.vm = new Vue({
            el: '#body',
            data: {
                latestBlocks: options.latestBlocks.reverse()
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