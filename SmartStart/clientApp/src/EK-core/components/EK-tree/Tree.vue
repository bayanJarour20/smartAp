<template>
    <div>
        <ul class="d-flex tree-row w-100 header border-bottom-0 border">
            <li class="text-center node-tree p-1" :style="i != 0 ? 'width: 220px!important' : 'flex-grow: 1; text-align: right!important; padding-right: 60px!important;'" v-for="(th, i) in header" :key="i">
                {{ t("positions").positionTable[th.label] }}
            </li>
            <li class="text-center node-tree p-1" style="width: 160px!important">
                {{ t('positions').actions.detailsposition }}
            </li>
            <li class="text-center node-tree p-1" style="width: 160px!important">
                {{ t('positions').actions.newposition }}
            </li>
        </ul>
        <div class="tree border">
            <ul class="tree-list">
                <node-tree v-for="(node, index) in treeData" :key="index" :level="1" :header="header" :node="node"></node-tree>
            </ul>
        </div>
    </div>
</template>

<script>
import NodeTree from "./NodeTree";
import { useUtils as useI18nUtils } from '@core/libs/i18n'

export default {
  props: {
    treeData: Array,
    header: Array
  },
    setup() {
        const { t } = useI18nUtils();
        return { t }
    },
  components: {
    NodeTree
  }
};
</script>

<style lang="scss">
.tree {
    overflow: hidden;
    ul {
        padding-left: 0;
        list-style: none;
    }
}
.header { 
    list-style: none;
    margin: 0;
    padding: 0;
}
.tree-list ul {
    padding: 0;
    margin: 0;
}
</style>