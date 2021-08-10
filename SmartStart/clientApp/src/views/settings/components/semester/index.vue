<template>
    <EKDialog @ok="onSubmit" @delete="deleteTag({id: tagsDto.id, type: 1})" :isEdit="!!tagsDto.id" ref="dialog" title="إضافة فصل">
        <template slot="body">
            <ValidationObserver ref="observer">
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم الفصل مطلوب' }]"
                    label="اسم الفصل"
                    v-model="tagsDto.name"
                    placeholder="ادخل اسم الفصل"
                    name="semester name"
                />
            </ValidationObserver>
        </template>
    </EKDialog>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKDialog from "@Ekcore/components/EK-dialog";
import { mapState, mapActions } from "vuex";
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText
    },
    computed: {
        ...mapState({
            tagsDto: state => state.globalStore.tagsDto
        })
    },
    methods: {
        ...mapActions(["createTag", "deleteTag"]),
        open() {
            this.$refs.dialog.open();
        },
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.tagsDto.type = 1;
                    this.createTag(this.tagsDto);
                }
            });
        }
    }
};
</script>
