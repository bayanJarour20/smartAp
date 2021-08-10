<template>
    <ValidationObserver ref="observer">
        <b-form @submit.prevent="onSubmit">
            <b-card no-body class="mb-2">
                <b-card-body>
                    <b-card-text>
                        <b-row>
                            <b-col cols="12" md="4">
                                <EKInputText
                                    v-model="feedbackDto.appUserName"
                                    label="اسم الطالب "
                                    readonly
                                    name="appUserName"
                                />
                            </b-col>
                            <b-col cols="12" md="4">
                                <EKInputText
                                    :value="moment(feedbackDto.sendDate).format('MMMM Do YYYY, h:mm:ss a')"
                                    label="تاريخ الإرسال"
                                    readonly
                                    name="sendDate"
                                />
                            </b-col>
                            <b-col cols="12" md="4">
                                <EKInputText
                                    :value="
                                        new Date(
                                            feedbackDto.replyDate
                                        ).getTime() ==
                                        new Date(
                                            '0001-01-01T00:00:00'
                                        ).getTime()
                                            ? ''
                                            : moment(feedbackDto.replyDate).format('MMMM Do YYYY, h:mm:ss a')
                                    "
                                    label="تاريخ الرد"
                                    placeholder="لم يتم الرد بعد"
                                    readonly
                                    name="replyDate"
                                />
                            </b-col>
                            <b-col cols="12" class="my-1">
                                <EKInputText
                                    v-model="feedbackDto.title"
                                    label="عنوان الرسالة"
                                    readonly
                                    name="title"
                                />
                            </b-col>
                            <b-col cols="12" class="my-1">
                                <EKInputTextarea
                                    v-model="feedbackDto.body"
                                    label="الرسالة"
                                    readonly
                                    name="body"
                                />
                            </b-col>
                            <b-col cols="12" class="my-1">
                                <EKInputTextarea
                                    v-model="feedbackDto.reply"
                                    label="الرد على الرسالة"
                                    :readonly="new Date(
                                            feedbackDto.replyDate
                                        ).getTime() !=
                                        new Date(
                                            '0001-01-01T00:00:00'
                                        ).getTime()"
                                    :rules="[
                                        { type: 'required', message: 'نص الرد مطلوب' }
                                    ]"
                                    placeholder="يمكنك كتابة رد هنا ..."
                                    name="reply"
                                />
                            </b-col>
                        </b-row>
                    </b-card-text>
                </b-card-body>
                <b-card-footer>
                    <b-row>
                        <b-col>
                            <div class="d-flex">
                                <b-button
                                    class="mr-1"
                                    type="submit"
                                    variant="primary"
                                    v-if="feedbackDto.replyDate!=null"
                                    style="max-width:100px"
                                    >إرسال رد</b-button
                                >
                                <b-button
                                    variant="outline-primary"
                                    style="max-width:100px"
                                    to="/contact"
                                    >تراجع</b-button
                                >
                            </div>
                        </b-col>
                        <b-col
                            style="
                                display: flex;
                                justify-content: flex-end;"
                        >
                            <b-button
                                @click="onDelete"
                                type="button"
                                style="max-width:100px"
                                variant="outline-danger"
                                >حذف</b-button
                            >
                        </b-col>
                    </b-row>
                </b-card-footer>
            </b-card>
        </b-form>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import { mapActions, mapState } from "vuex";
import moment from "moment";
export default {
    props: {
        id: String
    },
    created() {
        this.getFeedbackDetail(this.id);
    },
    components: {
        ValidationObserver,
        EKInputText,
        EKInputTextarea
    },
    computed: {
        ...mapState({
            feedbackDto: state => state.feedbacks.feedbackDto
        })
    },

    methods: {
        ...mapActions([
            "getFeedbackDetail",
            "actionFeedback",
            "deleteFeedback",
        ]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.feedbackDto.replyDate = new Date();
                    this.actionFeedback(this.feedbackDto);
                }
            });
        },
        moment,
        onDelete() {
            this.deleteFeedback(this.feedbackDto.id);
        }
    }
};
</script>
