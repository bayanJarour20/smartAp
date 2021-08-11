<template>
    <EKDialog @ok="onSubmit()" @delete="deleteCity(cityDto.id)" :isEdit="!!cityDto.id" ref="dialog" title="إضافة مدينة">
        <template slot="body">
            <ValidationObserver ref="observer">
                <EKInputText
                    :rules="[
                        { type: 'required', message: 'اسم المدينة مطلوب' }
                    ]"
                    label="اسم المدينة"
                    v-model="cityDto.name"
                    placeholder="ادخل اسم المدينة"
                    name="city name"
                />
            </ValidationObserver>
        </template>
    </EKDialog>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapState, mapActions } from "vuex";
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText
    },
    computed: {
        ...mapState({
            cityDto: state => state.globalStore.cityDto
        })
    },
    methods: {
        ...mapActions(["createCity", "deleteCity"]),
        open() {
            this.$refs.dialog.open();
        },
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.createCity(this.cityDto);
                }
            });
        }
    }
};
</script>
