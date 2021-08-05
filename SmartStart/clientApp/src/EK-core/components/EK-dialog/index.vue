<template>
<div>
    <slot name="activator">
       <activaitor @ok="is = true" @search="search" :placeholder="placeholder" :btnText="btnText"/>
    </slot>
    <b-sidebar backdrop shadow v-model="is">
        <template #header>
            <div
                class="d-flex justify-content-between align-items-center px-1 py-50 bg-light border-bottom"
            >
                <slot name="header">
                    <h5 class="m-0">
                        {{ title }}
                    </h5>
                </slot>
                <b-button
                    variant="flat-secondary"
                    size="sm"
                    class="rounded-circle btn-icon ml-auto"
                    @click="is = false"
                >
                    <unicon name="times" width="14" height="14" />
                </b-button>
            </div>
        </template>
        <template #default>
            <slot name="body">
            </slot>
        </template>
        <template #footer="{ hide }">
            <slot name="footer">
                <div class="d-flex align-items-center px-1 py-1 border-top">
                    <b-button variant="outline-danger" v-if="isEdit" @click="del">حذف</b-button>
                    <b-button
                        variant="outline-secondary"
                        class="ml-auto mr-50"
                        @click="hide"
                        >إلغاء</b-button
                    >
                    <b-button variant="primary" @click="ok">{{isEdit ? 'تعديل' : 'حفظ'}}</b-button>
                </div>
            </slot>
        </template>
    </b-sidebar>
</div>
</template>

<script>
import activaitor from "./activaitor";
import {
    BButton,
    BSidebar
} from "bootstrap-vue";
export default {
    components: {
        activaitor,
        // bootstrap vue
        BButton,
        BSidebar
    },
    data: () => ({
        is: false
    }),
    props: {
        placeholder: String,
        btnText: String,
        title: {
            type: String,
            default: () => "title"
        },
        endClose: Boolean,
        isEdit: {
            type: Boolean,
            default: () => false
        }
    },
    methods: {
        ok() {
            this.$emit('ok')
            if(this.endClose) this.is = false
        },
        del() {
            this.$emit('delete')
            this.is = false
        },
        search(e) {
            this.$emit('search', e)
        },
        open() {
            this.is = true
        },
        close() {
            this.is = false
        }
    },
    watch: {
        is (is) {
            if (is) { this.$emit('open') }
                else { this.$emit('close') }
        }
    }
};
</script>

<style lang="scss">
.b-sidebar-header {
  padding: 0!important;
  display: block!important;
}
.b-sidebar-body {
    padding: 12px;
}
</style>