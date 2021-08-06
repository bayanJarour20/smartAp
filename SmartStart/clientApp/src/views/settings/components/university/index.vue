<template>
<EKDialog @ok="onSubmit()" :isEdit="!!universityDto.id" @delete="deleteUniversity(universityDto.id)" ref="dialog" title="إضافة جامعة">
    <template slot="body">
        <ValidationObserver ref="observer">
            <EKInputText
                :rules="[{ type: 'required', message: 'اسم الجامعة مطلوب' }]"
                label="اسم الجامعة"
                v-model="universityDto.name"
                placeholder="ادخل اسم الجامعة"
                name="university name"
            />
            <EKInputSelect
                label="المدينة"
                placeholder="اختر المدينة"
                :rules="[{type: 'required', message: 'يجب تحديد حقل المدينة'}]"
                :options="citiesList"
                name="cityid"
                v-model="universityDto.cityId"
            />
        </ValidationObserver>
    </template>
</EKDialog>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapState, mapActions } from 'vuex';
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputSelect
    },
    computed: {
        ...mapState({
            citiesList: state => state.globalStore.citiesList,
            universityDto: state => state.globalStore.universityDto
        })
    },
    methods: {
        ...mapActions(["createUniversity", "deleteUniversity"]),
        open() {
            this.$refs.dialog.open()
        },
        onSubmit() {
            this.$refs.observer.validate().then((success) => {
                if(success) {
                    this.createUniversity(this.universityDto)
                }
            })
        }
    }
}
</script>