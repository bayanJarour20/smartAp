<template>
    <ValidationObserver ref="observer">
        <EKDialog
            ref="notificationDialog"
            :title="!isEdit ? 'إرسال إشعار' : 'تفاصيل الإشعار'"
            :placeholder="!isEdit ? 'ابحث عن إشعار ما...' : ''"
            :btnText="!isEdit ? 'إرسال إشعار' : ''"
            @ok="onSubmit"
            @close="$store.commit('Reset_Notification_Dto')"
            :isEdit="isEdit"
            @delete="deleteNotification(notificationDto.id)"
            @search="search"
        >
            <template #body>
                <div v-if="!isEdit">
                    <label>إرسال إلى</label>
                    <div class="d-flex align-items-center my-50">
                        <label class="m-0">الطلاب</label>
                        <b-checkbox @change="notificationDto.userIds = []" switch v-model="notificationDto.notificationType"></b-checkbox>
                        <label class="m-0">نقاط البيع</label>
                    </div>
                </div>
                <label class="mb-50">فلترة حسب : </label>
                <EKInputSelect
                    v-if="!notificationDto.notificationType"
                    label="كلية"
                    placeholder="اختر الكلية"
                    :options="faculties"
                    name="selectfactely"
                    :clearable="true"
                    v-model="notificationFilterDto.faculityId"
                />
                <EKInputSelect
                    v-else
                    label="المدينة"
                    placeholder="اختر المدينة"
                    :options="citiesList"
                    name="selectciies"
                    v-model="notificationFilterDto.cityId"
                    :clearable="true"
                />
                <EKInputSelect
                    v-if="!notificationDto.notificationType"
                    label="الطالب"
                    placeholder="اختر الطالب"
                    name="selectstudent"
                    :options="filterdUserListForNotification"
                    multiple
                    v-model="notificationDto.userIds"
                    :clearable="true"
                />
                
                <EKInputSelect
                    v-else
                    label="نقطة البيع"
                    placeholder="اختر نقطة البيع"
                    name="selectsell"
                    multiple
                    :options="filterdUserListForNotification"
                    v-model="notificationDto.userIds"
                    :clearable="true"
                />
                <hr>
                <EKInputText
                    :rules="[{ type: 'required', message: ' العنوان مطلوب' }]"
                    label="عنوان الإشعار"
                    v-model="notificationDto.title"
                    placeholder="ادخل العنوان"
                    name="title"
                />
                <EKInputTextarea
                    :rules="[{ type: 'required', message: ' النص إجباري' }]"
                    label="نص الإشعار"
                    v-model="notificationDto.body"
                    placeholder="ادخل النص"
                    name="body"
                />
            </template>
        </EKDialog>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import { mapActions, mapState, mapGetters } from "vuex";

export default {
    props: {
        isEdit: Boolean
    },
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputSelect,
        EKInputTextarea
    },
    data() {
        return {
            currentState: false
        };
    },
    computed: {
        ...mapGetters(["filterdUserListForNotification", "tagsList", "semester", "doctors"]),
        ...mapState({
            notificationDto: state => state.natification.notificationDto,
            notificationFilterDto: state => state.natification.notificationFilterDto,
            faculties: state => state.faculties.faculties,
            citiesList: state => state.globalStore.citiesList
        })
    },
    created() {
        this.getFacultiesDetails()
        this.fetchCity()
        this.fetchTotalTag()
        this.getUsers(0)
       
    },
    methods: {
        ...mapActions(["deleteNotification", "newNotification", "getFacultiesDetails", "fetchCity", "fetchTotalTag", "getUsers"]),
        onSubmit() {
            this.$refs.observer.validate().then(success => {
                if (success) {
                    this.newNotification({
                        title: this.notificationDto.title,
                        body: this.notificationDto.body,
                        date: this.notificationDto.date,
                        notificationType: +this.notificationDto.notificationType,
                        userIds: this.notificationDto.userIds
                    });
                }
            });
        },
        open() {
            this.$refs.notificationDialog.open()
        },
        search(query) {
            this.$store.commit('Set_Search_Dto', {
                keys: ["title", 'body'],
                query   
            })
        }
    },
    watch: {
        'notificationDto.notificationType'(val) {
            this.notificationFilterDto.faculityId = 0
            this.notificationFilterDto.cityId = 0
            this.getUsers(+val)
        },
        'notificationFilterDto.faculityId'(){
            this.notificationDto.userIds=[]

        }
    }
};
</script>
