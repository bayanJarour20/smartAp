<template>
    <b-form-group v-bind="$attrs" :label-for="'label-for-picker-range-' + id">
        <b-input-group>
            <flat-pickr
                :id="'label-for-picker-range-' + id"
                @on-change="onChange"
                v-model="innerVal"
                :placeholder="placeholder"
                class="form-control text-left"
                dir="ltr"
                v-bind="$attrs"
                :config="config"
            />
            <template #append>
                <b-input-group-text>
                    <unicon
                        name="calendar-alt"
                        width="18"
                        height="18"
                        fill="#B9B9C3"
                    />
                </b-input-group-text>
            </template>
        </b-input-group>
        <small class="text-danger" v-if="required && !innerVal">{{name ? name : 'الحقل'}} مطلوبة</small>
        <small class="text-danger" v-if="!!innerVal && innerVal.split('to').length < 2">{{'الحقل يجب ان يكون من الشكل : yyyy-mm-dd to yyyy-mm-dd'}} مطلوبة</small>
    </b-form-group>
</template>

<script>
import flatPickr from "vue-flatpickr-component";
import { BFormGroup, BInputGroup, BInputGroupText } from "bootstrap-vue";

export default {
    components: {
        flatPickr,
        // bootstrap vue
        BFormGroup,
        BInputGroup,
        BInputGroupText
    },
    props: {
        placeholder: String,
        type: String,
        required: Boolean,
        name: String,
        value: {
            type: null
        }
    },
    mounted() {
        if (this.value) {
            this.innerVal = this.value;
        }
    },
    data: () => ({
        innerVal: "",
        id: Math.random() * 100000,
        config: {
            allowInput: true,
            mode: "range"
        }
    }),
    methods: {
        onChange(v) {
            this.$emit("onChange", v);
        }
    },
    watch: {
        innerVal(v) {
            this.$emit("input", v);
        },
        value(v) {
            this.innerVal = v;
        }
    }
};
</script>
