<template>
    <ValidationObserver ref="observerMicroscopeSection">
        <EKDialog
            ref="microscopeSectionDialog"
            @ok="onSubmit()"
            title="إضافة سؤال"
            btnText="سؤال جديد"
           
        >
            <template #body>
                <EKInputText
                    :rules="[
                        {
                            type: 'required',
                            message: 'عنوان السؤال إجباري'
                        }
                    ]"
                    label="عنوان السؤال"
                    placeholder="ادخل عنوان السؤال"
                    name="name"
                    v-model="title"
                />
            </template>
        </EKDialog>
    </ValidationObserver>
</template>
<script>
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapActions } from "vuex";
import { ValidationObserver } from "vee-validate";
export default {
    components: {
        EKDialog,
        EKInputText,
        ValidationObserver
    },
    props: {
        id: String
    },
    data: () => ({
        title: ""
    }),

    methods: {
        ...mapActions(["addSectionsMicroscope"]),
        onSubmit() {
            this.$refs.observerMicroscopeSection.validate().then(success => {
                if (success) {
                    var data = new FormData();

                    data.append("Id", `${this.id}`);
                    data.append("Sections[0].Order", "1");
                    data.append("Sections[0].Title", `${this.title}`);
                    data.append("Sections[0].QuestionType", "1");
                    data.append("Sections[0].AnswerType", "1");

                    this.addSectionsMicroscope(data);
                    this.title = "";
                    this.$refs.microscopeSectionDialog.close();
                }
            });
        }
    }
};
</script>
