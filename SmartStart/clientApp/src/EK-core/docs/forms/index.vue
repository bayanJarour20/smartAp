<template>
<b-container class="p-4">
    <b-row>
        <b-col cols="12" md="6">
            <EKInputText
                :rules="[{type: 'required', message: 'الحقل إجباري'}, {type: 'min:4', message: 'min is 4'}]"
                label="label"
                placeholder="placeholder"
                name="lable"
            />
        </b-col>
        <b-col cols="12" md="6">
            <EKInputSelect
                label="label"
                placeholder="placeholder"
                :rules="[{type: 'required', message: 'الحقل إجباري'}]"
                :options="[{id: 1, name: 'asa'}, {id: 2, name: 'asfs'}]"
                name="select"
                :clearable="true"
            />
            <!-- textLabel valueLabel -->
        </b-col>
        <b-col cols="12" md="6">
            <EKInputImage
                label="label"
                base64
                mul
                required
                title="upload image"
                v-model="base64"
            ></EKInputImage>
        </b-col>
        <b-col cols="12" md="6">
            <EKInputPicker
                label="datepicker"
                :rules="[{type: 'required', message: 'الحقل إجباري'}]"
                name="picker"
                placeholder="Choose a date"
            >
            </EKInputPicker>
        </b-col>
        <b-col cols="12" md="6">
            <EKInputPickerRange
                label="range datepicker"
                placeholder='placeholder'
                required
            >
            </EKInputPickerRange>
        </b-col>
    </b-row>
</b-container>

</template>
<script>
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import EKInputPicker from "@Ekcore/components/EK-forms/EK-input-picker";
import EKInputPickerRange from "@Ekcore/components/EK-forms/EK-input-picker-range";
export default {
    components: {
        EKInputText,
        EKInputSelect,
        EKInputImage,
        EKInputPicker,
        EKInputPickerRange
    },
    data: () => ({
        base64: null,
    })
}
</script>