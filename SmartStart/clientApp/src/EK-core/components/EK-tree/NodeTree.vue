<template>
    <div class="w-100">
        <div class="d-flex pl-2 w-100 border tree-node-container">
            <li
                v-for="(lev, index) in level"
                :key="index"
                class="p-1"
                :class="{ 'tree-node-before': index == level - 1 }"
            ></li>
            <li
                class="node-tree py-1 px-2 position-relative"
                :style="(i != 0 ? 'width: 220px!important; text-align: center;' : 'flex-grow: 1') +
                        (node.isManager ? 'font-weight: bold; color: #7367f0' : '')
                "
                v-for="(th, i) in header"
                :key="'before' + level * i"
            >
                <transition
                    name="fade" 
                >
                    <b-button
                        v-if="i == 0 && node.positionDtos == null && node.isParent"
                        size="sm"
                        variant="light"
                        class="rounded-circle tree-sync-btn"
                        style="padding: 2px 6px!important; margin: auto"
                        @click="node.positionDtos = []; getPositions(node.id)"
                    >
                        <unicon name="sync" fill="#82868b" width="16" height="23"></unicon>
                    </b-button>
                    <b-button
                        v-else-if="i == 0 && node.isChild && node.isParent"
                        size="sm"
                        variant="flat"
                        class="rounded-circle tree-collapse-btn"
                        style="padding: 2px 6px!important; margin: auto"
                        @click="isColapced = !isColapced"
                    >
                        <unicon :name="isColapced ? 'angle-down' : 'angle-up'" fill="#82868b" width="20" height="27"></unicon>
                    </b-button>
                </transition>
                {{ node[th.value] }}
            </li>
            <li
                class="node-tree d-flex align-items-center"
                style="width: 160px!important; text-align: center;"
            >
                <b-button
                    size="sm"
                    variant="flat-secondary"
                    class="btn-icon rounded-circle"
                    style="padding: 2px 6px!important; margin: auto"
                    :to="'company-positions/0/' + node.id"
                >
                    <unicon name="pen" :fill="node.isManager ? '#7367f0' : '#000000'"  width="20" height="27"></unicon>
                </b-button>
            </li>
            <li
                class="node-tree d-flex align-items-center"
                style="width: 160px!important; text-align: center;"
            >
                <b-button
                    v-if="node.isChild"
                    size="sm"
                    variant="flat-secondary"
                    class="btn-icon rounded-circle"
                    style="padding: 2px 6px!important; margin: auto"
                    :to="'company-positions/1/' + node.id"
                >
                    <unicon name="plus" width="20" height="27"></unicon>
                </b-button>
            </li>
        </div>
        <template v-if="node.positionDtos && node.positionDtos.length">
            <template class="pl-2" v-for="(child, index) in node.positionDtos">
                <transition
                    name="fade" 
                    :key="index"
                >
                    <node
                        v-show="isColapced"
                        :level='level+1'
                        :header="header"
                        :node="child"
                    ></node>
                </transition>
            </template>
        </template>
    </div>
</template>

<script>
import { BButton } from "bootstrap-vue";
import { mapActions } from 'vuex';
export default {
    name: "node",
    components: {
        BButton
    },
    props: {
        node: Object,
        header: Array,
        level: Number,
    },
    methods: {
        ...mapActions(["getPositions"])
    },
    data: () => ({
        isColapced: false
    })
};
</script>

<style lang="scss" scoped>
.tree-node-container {
    .tree-node-before {
        position: relative;
        &:before {
            background: rgba(0, 0, 0, 0.375);
            height: 102%;
            width: 1px;
            content: "";
            position: absolute;
            top: 0;
            right: 14px;
            transform: translateY(-50%);
        }
        &:after {
            background: rgba(0, 0, 0, 0.375);
            height: 1px;
            width: 28px;
            content: "";
            position: absolute;
            top: 50%;
            left: 14px;
            transform: translateY(-50%);
        }
    }
}
.last-item-child {
    background: blue;
}
.tree-sync-btn {
    position: absolute;
    top: 50%;
    transform: translate(-112%, -15px)
}
.tree-collapse-btn {
    position: absolute;
    top: 50%;
    transform: translate(-200%, -16px)
}
</style>
