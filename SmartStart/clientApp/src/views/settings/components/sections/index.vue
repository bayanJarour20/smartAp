<template>
    <EKDialog @ok="onSubmit()" @delete="deleteTag({id: tagsDto.id, type: 4})" :isEdit="!!tagsDto.id" ref="dialog" title="إضافة قسم">
        <template slot="body">
            <ValidationObserver ref="observer">
                <EKInputText
                    :rules="[{ type: 'required', message: 'اسم القسم مطلوب' }]"
                    label="اسم القسم"
                    v-model="tagsDto.name"
                    placeholder="ادخل اسم القسم"
                    name="team name"
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
                    this.tagsDto.type = 4;
                    this.createTag(this.tagsDto);
                }
            });
        }
    }
};
</script>
