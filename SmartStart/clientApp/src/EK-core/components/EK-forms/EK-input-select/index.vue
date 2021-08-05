<template>
<ValidationProvider
    #default="{ errors }" 
    :name="name"
    :rules="faildRules"
>
    <b-form-group
        v-bind="$attrs"
        :label-for="'label-for-select-' + id"
    >
        <v-select
            :id="'label-for-select-' + id"
            v-model="innerVal"
            :options="options"
            :reduce="item => valueLabel ? item[valueLabel] : item"
            :label="textLabel"
            :dir="$store.state.appConfig.layout.isRTL ? 'rtl' : 'ltr'"
            :clearable="clearable"
            :multiple="multiple"
            :placeholder="placeholder"
            class="rounded"
            :class="{'border-danger': errors[0]}"
        >
            <template #list-header>
                <slot name="list-header"></slot>
            </template>
            <template #option="{name}">
                <slot name="option" :label="name"></slot>
            </template>
            <template #selected-option="{name}">
                <slot name="selected-option" :label="name"></slot>
            </template>
            <template #list-footer>
                <slot name="list-footer"></slot>
            </template>
        </v-select>
        <small class="text-danger">{{ errors[0] }}</small>
    </b-form-group>
</ValidationProvider>
</template>
<script>
import { ValidationProvider, localize } from "vee-validate";
import { BFormGroup } from "bootstrap-vue";
import vSelect from "vue-select";
import ar from "vee-validate/dist/locale/ar.json";
import {
    required,
    email,
    min,
    confirmed,
    regex,
    url,
    credit,
    password,
    between,
    alpha,
    integer,
    digits,
    alphaDash,
    alphaNum,
    length,
    no_spaces,
    positive
} from "@validations";
export default {
    components: {
        vSelect,
        ValidationProvider,
        // bootstrap vue
        BFormGroup
    },
    props: {
        options: Array,
        clearable: {
            type: Boolean,
            default: () => false
        },
        placeholder: {
            type: String,
            default: 'اختر عنصر'
        },
        valueLabel: {
            type: String,
            default: () => 'id'
        },
        textLabel: {
            type: String,
            default: 'name'
        },
        multiple:Boolean,
        value: {
            type: null
        },

        name: {
            type: String,
            required: true
        },
        vid: {
            type: String
        },
        rules: {
            type: Array,
            default: () => []
        },
    },
    setup(props) {
        let faildRules = ''
        let fields = {}
        props.rules.forEach(rule => {
            faildRules += rule.type + '|'
            fields[rule.type.split(':')[0]] = rule.message
        });
        faildRules = faildRules.slice(0, faildRules.length-1)
        localize('ar', {
            messages: ar.messages,
            fields: {
                [props.name]: fields
            }
        });
        return { faildRules}
    },
    mounted() {
        if (this.value) {
            this.innerVal = this.value;
        }
    },
    data: () => ({
        innerVal: null,
        id: Math.random() * 100000,

        // validation
        required,
        email,
        min,
        confirmed,
        regex,
        url,
        credit,
        password,
        between,
        alpha,
        integer,
        digits,
        alphaDash,
        alphaNum,
        length,
        no_spaces,
        positive
    }),
    watch: {
        innerVal(v) {
            this.$emit("input", v);
        },
        value(v) {
            this.innerVal = v;
        }
    }
}
</script>
<style lang="scss" scoped>
.v-select {
    border: solid 1px #d8d6de;
}
::v-deep .vs__dropdown-toggle {
    border: none;
    background: #ffff;
    input::placeholder {
        color: #B9BFD3
    }
}
</style>