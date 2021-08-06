<template>
    <EKDialog @ok="onSubmit()" @delete="deleteTag({id: tagsDto.id, type: 0})" :isEdit="!!tagsDto.id" ref="dialog" title="إضافة وسم">
        <template slot="body">
            <ValidationObserver ref="observer">
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم الوسم مطلوب' }]"
                    label="اسم الوسم"
                    v-model="tagsDto.name"
                    placeholder="ادخل اسم الوسم"
                    name="tag name"
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
                    this.tagsDto.type = 0;
                    this.createTag(this.tagsDto);
                }
            });
        }
    }
};
</script>
