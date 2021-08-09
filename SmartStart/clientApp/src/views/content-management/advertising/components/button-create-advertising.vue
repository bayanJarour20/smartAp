<template>
    <ValidationObserver ref="observerAdvertising">
        <b-form>
            <EKDialog
                ref="editAdvertisingeDialog"
                @ok="onSubmit()"
                @close="closeDialog()"
                :title="title"
                :endClose="advertisingDto.id != ''"
                :placeholder="!isEdit ? 'ابحث عن إعلان محددة' : ''"
                :btnText="!isEdit ? 'إعلان جديد' : ''"
                @search="search"
            >
                <template #body>
                    
                    <EKInputText
                        :rules="[
                            {
                                type: 'required',
                                message: 'العنوان الرئيسي إجباري'
                            }
                        ]"
                        label="العنوان الرئيسي"
                        placeholder="ادخل العنوان الرئيسي"
                        name="name"
                        v-model="advertisingDto.title"
                    />
                    <EKInputPickerRange
                        required
                        label="مدة الإعلان"
                        name="مدة الإعلان"
                        placeholder="Choose a date"
                        v-model="advertisingDtoDate"
                    >
                    </EKInputPickerRange>
                    <EKInputImage
                        label='صورة الإعلان'
                        required
                        title="تحميل صورة"
                        @input="advertisingDto.imageFile = $event"
                        :value="advertisingDto.imagePath
                                    ? $store.getters['app/domainHost'] +
                                        '/' +
                                        advertisingDto.imagePath : ''
                                "
                    ></EKInputImage>
                
                </template>
                <template #footer>
                    <div class="d-flex align-items-center px-1 py-1 border-top">
                        <b-button
                            variant="outline-danger"
                            @click="deleteAdvertising(advertisingDto.id)"
                            v-show="isEdit"
                            >حذف</b-button
                        >
                        <b-button
                            variant="outline-secondary"
                            class="ml-auto mr-50"
                            @click="closeDialog()"
                            >إلغاء</b-button
                        >
                        <b-button variant="primary" @click="onSubmit()"
                            >حفظ</b-button
                        >
                    </div>
                </template>
            </EKDialog>
        </b-form>
    </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKDialog from "@Ekcore/components/EK-dialog";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputImage from "@Ekcore/components/EK-forms/EK-input-image";
import EKInputPickerRange from "@Ekcore/components/EK-forms/EK-input-picker-range";
import { mapActions, mapState } from "vuex";
export default {
    components: {
        ValidationObserver,
        EKDialog,
        EKInputText,
        EKInputImage,
        EKInputPickerRange
    },
    computed: {
        ...mapState({
            advertisingDto: state => state.advertising.advertisingDto
        })
    },
    data: () => ({
        advertisingDtoDate: ""
    }),

    props: {
        title: {
            type: String,
            default: () => "إضافة إعلان"
        },
        isEdit: Boolean
    },
    methods: {
        ...mapActions(["addAdvertising", "deleteAdvertising"]),
        onSubmit() {
            this.$refs.observerAdvertising.validate().then(success => {
                let datesArr = this.advertisingDtoDate.split("to");
                if (
                    success &&
                    (this.advertisingDto.imageFile || this.advertisingDto.imagePath) &&
                    (((this.advertisingDto.videoFile || this.advertisingDto.videoPath) || this.advertisingDto.type == 0)) &&
                    datesArr.length == 2
                ) {
                    this.advertisingDto.startDate = new Date(datesArr[0]);
                    this.advertisingDto.endDate = new Date(datesArr[1]);
                    var AdsFormData = new FormData();
                    if (this.advertisingDto.id == "") {
                        AdsFormData.append("type", this.advertisingDto.type);
                        AdsFormData.append(
                            "imageFile",
                            this.advertisingDto.imageFile
                        );
                        AdsFormData.append("title", this.advertisingDto.title);
                        if(this.advertisingDto.type == 2) {
                            AdsFormData.append(
                                "videoFile",
                                this.advertisingDto.videoFile
                            );
                            AdsFormData.append(
                                "videoPath",
                                this.advertisingDto.videoPath
                            );
                        }
                        AdsFormData.append(
                            "imagePath",
                            this.advertisingDto.imagePath
                        );
                        AdsFormData.append(
                            "startDate",
                            new Date(
                                this.advertisingDto.startDate
                            ).toLocaleDateString()
                        );
                        AdsFormData.append(
                            "endDate",
                            new Date(
                                this.advertisingDto.endDate
                            ).toLocaleDateString()
                        );
                    } else {
                        AdsFormData.append("id", this.advertisingDto.id);
                        AdsFormData.append(
                            "type",
                            this.advertisingDto.type
                        );
                        AdsFormData.append(
                            "imageFile",
                            this.advertisingDto.imageFile
                        );
                        AdsFormData.append("title", this.advertisingDto.title);
                        if(this.advertisingDto.type == 2) {
                            AdsFormData.append(
                                "videoFile",
                                this.advertisingDto.videoFile
                            );
                            AdsFormData.append(
                                "videoPath",
                                this.advertisingDto.videoPath
                            );
                        }
                        AdsFormData.append(
                            "imagePath",
                            this.advertisingDto.imagePath
                        );
                        AdsFormData.append(
                            "startDate",
                            new Date(
                                this.advertisingDto.startDate
                            ).toLocaleDateString()
                        );
                        AdsFormData.append(
                            "endDate",
                            new Date(
                                this.advertisingDto.endDate
                            ).toLocaleDateString()
                        );
                    }

                    this.addAdvertising({
                        id: this.advertisingDto.id,
                        formData: AdsFormData
                    });
                    this.closeDialog();
                }
            });
        },
        openDialog() {
            if (this.isEdit) {
                this.advertisingDtoDate =
                    this.advertisingDto.startDate +
                    " to " +
                    this.advertisingDto.endDate;
            }
            this.$refs.editAdvertisingeDialog.open();
        },
        closeDialog() {
            this.$store.commit("Reset_Advertising");
            this.$refs.editAdvertisingeDialog.close();
            this.advertisingDtoDate = "";
        },
        search(query) {
            this.$store.commit("Set_Search_Dto", {
                keys: ["title"],
                query
            });
        }
    }
};
</script>
