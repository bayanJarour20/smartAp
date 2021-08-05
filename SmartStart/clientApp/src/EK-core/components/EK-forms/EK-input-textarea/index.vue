<template>
    <ValidationProvider :vid="vid" :name="name" :rules="faildRules">
        <b-form-group
            slot-scope="{valid, errors }"
            v-bind="$attrs"
            :label-for="'label-for-text-' + id"
        >
            <b-form-textarea
                :id="'label-for-text-' + id"
                v-model="innerVal"
                v-bind="$attrs"
                trim
                :type="type"
                :state="faildRules ? (errors[0] ? false : valid ? true : null) : null"
            ></b-form-textarea>
            <b-form-invalid-feedback>
                {{ errors[0] }}
            </b-form-invalid-feedback>
        </b-form-group>
    </ValidationProvider>
</template>

<script>
import { ValidationProvider, localize } from "vee-validate";
import { BFormGroup, BFormTextarea, BFormInvalidFeedback } from "bootstrap-vue";
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
        ValidationProvider,
        // bootstrap vue
        BFormGroup,
        BFormTextarea,
        BFormInvalidFeedback
    },
    props: {
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
        type: String,
        value: {
            type: null
        }
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
        innerVal: "",
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
            this.$emit("input", this.type == 'number' ? +v : (v ? v.trim() : ''));
        },
        value(v) {
            this.innerVal = v;
        }
    }
};
</script>
