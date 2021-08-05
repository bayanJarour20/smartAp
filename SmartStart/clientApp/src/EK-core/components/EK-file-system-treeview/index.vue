<template>
    <b-form-checkbox-group v-model="selected">
        <tree-item
            class="item"
            :item="data"
            @make-folder="makeFolder"
            @add-item="addItem"
        ></tree-item>
    </b-form-checkbox-group>
</template>
<script>
import treeItem from "./tree-item.vue";
import {
    BFormCheckboxGroup
} from "bootstrap-vue";
export default {
    props: {
        data: Object,
    },
    data: () => ({
        selected: []
    }),
    components: {
        treeItem,
        BFormCheckboxGroup
    },
    methods: {
        makeFolder: function(item) {
            this.$set(item, "children", []);
            this.addItem(item);
        },
        addItem: function(item) {
            item.children.push({
                name: "new stuff"
            });
        }
    },
    watch: {
        selected(val) {
            this.$emit('input', val)
        }
    }
};
</script>
