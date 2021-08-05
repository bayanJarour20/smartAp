<template>
     <div class="d-flex justify-content-end">
        <b-input-group
            class="input-group-merge"
            style="margin: auto; margin-left: 20px;"
            v-if="!!placeholder"
        >
            <b-form-input @input="search" type="search" :placeholder="placeholder" />
            <b-input-group-append is-text>
                <unicon width="16" height="16" fill="gray" name="search" />
            </b-input-group-append>
        </b-input-group>
        <slot>
            <b-button
                v-if="!!btnText"
                size="sm"
                variant="primary"
                class="text-nowrap"
                @click="ok"
            >
                {{ btnText }}
                <unicon
                    class="ml-1"
                    name="plus"
                    width="18"
                    height="18"
                    fill="#fff"
                />
            </b-button>
        </slot>
    </div>
</template>
<script>
import {
    BInputGroup,
    BFormInput,
    BInputGroupAppend
} from "bootstrap-vue";
export default {
    components: {
        BInputGroup,
        BFormInput,
        BInputGroupAppend,
    },
    props: {
        btnText: String,
        placeholder: String
    },
    methods: {
        search(e) {
            this.$emit('search', e)
        },
        ok() {
            this.$emit('ok')
        }
    }
}
</script>