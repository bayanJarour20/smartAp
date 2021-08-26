<template>
  <ValidationObserver ref="observer">
    <b-form>
      <b-row>
        <b-col cols="12">
          <b-card no-body class="mb-2">
            <b-card-header class="align-items-center">
              <h4 class="mr-auto mb-0">تفاصيل المجهر</h4>
            </b-card-header>
            <ValidationObserver ref="observertelescope">
              <b-card-body>
                <b-card-text>
                  <b-row>
                    <b-col cols="12" md="6">
                      <EKInputSelect
                        label="المادة"
                        placeholder="اختر المادة"
                        :rules="[
                          {
                            type: 'required',
                            message: 'المادة مطلوب',
                          },
                        ]"
                        :options="subjectsList"
                        v-model="telescopeDto.subjectId"
                        name="subjectId"
                        :clearable="true"
                      />
                    </b-col>
                    <b-col cols="12" md="6">
                      <EKInputText
                        label="اسم المجهر"
                        placeholder="اختر اسم المجهر"
                        :rules="[
                          {
                            type: 'required',
                            message: 'اسم المجهر مطلوب',
                          },
                        ]"
                        v-model="telescopeDto.name"
                        name="name"
                        :clearable="true"
                      />
                    </b-col>
                    <b-col cols="12" md="6">
                      <EKInputSelect
                        label="السنة"
                        placeholder="اختر السنة"
                        :rules="[
                          {
                            type: 'required',
                            message: 'السنة إجبارية',
                          },
                        ]"
                        :options="years"
                        v-model="telescopeDto.year"
                        name="year"
                      />
                    </b-col>
                    <b-col cols="12" md="6">
                      <EKInputText
                        :rules="[
                          {
                            type: 'required',
                            message: 'السعر الإفتراضي إجباري',
                          },
                          {
                            type: 'min_value:0',
                            message: 'الحقل يجب ان يكون بقيمة موجبة',
                          },
                        ]"
                        label="السعر الإفتراضي"
                        placeholder="ادخل السعر الإفتراضي"
                        name="price"
                        type="number"
                        v-model="telescopeDto.price"
                      />
                    </b-col>
                    <b-col cols="12" md="6">
                      <EKInputSelect
                        label="تصنيف المجهر"
                        placeholder="اختر تصنيفات"
                        :rules="[
                          {
                            type: 'required',
                            message: 'التصنيف إجباري',
                          },
                        ]"
                        :options="tagsList"
                        v-model="telescopeDto.tags"
                        multiple
                        name="tags"
                      />
                    </b-col>
                    <b-col cols="12" md="6">
                      <EKInputSelect
                        label="دكتور المجهر"
                        placeholder="اختر الدكتور"
                        :rules="[
                          {
                            type: 'required',
                            message: 'الدكتور إجباري',
                          },
                        ]"
                        multiple
                        :options="doctors"
                        v-model="telescopeDto.doctors"
                        name="doctor"
                      />
                    </b-col>
                  </b-row>
                </b-card-text>
              </b-card-body>
              <b-card-footer class="d-flex">
                <b-button variant="primary" @click="onSubmit()">تعديل</b-button>
                <b-button variant="outline-primary" class="mx-1" to="/telescope"
                  >تراجع</b-button
                >
                <b-button
                  variant="outline-danger"
                  class="ml-auto"
                  @click="onRemove()"
                  >حذف</b-button
                >
              </b-card-footer>
            </ValidationObserver>
          </b-card>
        </b-col>
        <b-col cols="12">
          <b-card no-body class="mb-2">
            <b-card-header class="align-items-center">
              <h4 class="mr-auto mb-0">صور المجهر</h4>
              <addMicroscopeSection
                ref="microscopeSectionDialog"
                :id="telescopeDto.id"
              />
            </b-card-header>
            <ValidationObserver ref="sectionMicroscope">
              <b-card-body>
                <b-card-text>
                  <b-row>
                    <b-col cols="12" class="d-flex align-items-center">
                      <EKInputSelect
                        class="flex-grow-1"
                        label="السؤال"
                        placeholder="اختر سؤال"
                        :options="telescopeDto.sections"
                        v-model="selectedSection"
                        name="selectedSection"
                        textLabel="title"
                        :clearable="true"
                        :rules="[
                          {
                            type: 'required',
                            message: 'السؤال إجباري',
                          },
                        ]"
                      />
                      <b-button
                        v-if="selectedSection"
                        variant="warning"
                        @click="OnDeleteSectionMicroscope(selectedSection)"
                        class="ml-1 mt-1 py-75 px-1"
                        ><unicon
                          name="trash-alt"
                          fill="#fff"
                          width="18"
                          height="18"
                      /></b-button>
                    </b-col>
                    <b-col cols="12">
                      <input-gallery
                        :base64="false"
                        required
                        title="صورة جديدة"
                        :mul="true"
                        height="150px"
                        @input="uploadImages($event)"
                      >
                      </input-gallery>
                    </b-col>
                    <b-col
                      cols="3"
                      v-for="(doc, index) in selectedSectionDto.documents"
                      :key="index"
                    >
                      <div
                        class="
                          drop-container
                          rounded
                          overflow-hidden
                          d-flex
                          justify-content-center
                          align-items-center
                          mt-2
                        "
                        style="
                          height: 150px;
                          box-shadow: 0px 0px 5px 0px #0000003d;
                          background-size: cover;
                          position: relative; 
                        "
                        :style="
                          (indexChoose == index && typeListDescription == 0
                            ? 'border: #6610f2; border-style: solid;'
                            : '') +
                          'background-image:' +
                          (doc.isDeleted
                            ? 'linear-gradient(rgb(100 0 0 / 65%), rgb(100 0 0 / 65%)),'
                            : '') +
                          'url(' +
                          $store.getters['app/domainHost'] +
                          '/' +
                          doc.path
                            .replaceAll('\\', '/')
                            .replaceAll(' ', '%20') +
                          ');' +
                          (doc.isDeleted ? '' : 'cursor: pointer;')
                        "
                        @click="chooseImage(index)"
                      >
                        <b-button
                          size="sm"
                          class="clear btn-icon border-0 mr-1"
                          style="margin-top: 4px; z-index: 1; position: absolute; top: 0px; left: 0px;"
                          variant="flat-secondary"
                          @click="toggleImage(index, doc)"
                        >
                        {{doc.isDeleted}}
                          <unicon
                            :name="doc.isDeleted ? 'history-alt' : 'trash-alt'"
                            :fill="doc.isDeleted ? '#11cc22' : '#ea5455'"
                            width="16"
                            height="16"
                          />
                        </b-button>
                      </div>
                    </b-col>
                    <b-col cols="12">
                      <EKInputTextarea
                        v-if="
                          selectedSectionDto.documents.length > 0 &&
                          typeListDescription == 0
                        "
                        label=" وصف الصورة"
                        placeholder="وصف الصورة مطلوب"
                        name="name"
                        v-model="selectedSectionDto.documents[indexChoose].note"
                      />
                      <EKInputTextarea
                        v-else-if="
                          selectedSectionDto.documents.length > 0 &&
                          typeListDescription == 1
                        "
                        :rules="[
                          {
                            type: 'required',
                            message: 'وصف الصورة مطلوب',
                          },
                        ]"
                        label=" وصف الصورة"
                        placeholder="وصف الصورة مطلوب"
                        name="name"
                        v-model="newDocuments[documentIndex].note"
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
                        variant="primary"
                        style="max-width: 100px"
                        @click="SubmitUpdateSectionMicroscope"
                        >حفظ</b-button
                      >
                      <b-button
                        variant="outline-primary"
                        style="max-width: 100px"
                        to="../telescope"
                        >تراجع</b-button
                      >
                    </div>
                  </b-col>
                  <b-col style="display: flex; justify-content: flex-end">
                    <b-button style="max-width: 100px" variant="outline-primary"
                      >حذف</b-button
                    >
                  </b-col>
                </b-row>
              </b-card-footer>
            </ValidationObserver>
          </b-card>
        </b-col>
      </b-row>
    </b-form>
  </ValidationObserver>
</template>
<script>
import { ValidationObserver } from "vee-validate";
import EKInputSelect from "@Ekcore/components/EK-forms/EK-input-select";
import EKInputText from "@Ekcore/components/EK-forms/EK-input-text";
import EKInputTextarea from "@Ekcore/components/EK-forms/EK-input-textarea";
import addMicroscopeSection from "./../components/add-telecsopesection-modal.vue";
import { mapActions, mapState, mapGetters, mapMutations } from "vuex";
import inputGallery from "./../components/input-gallery.vue";

export default {
  components: {
    ValidationObserver,
    EKInputSelect,
    EKInputText,
    EKInputTextarea,

    addMicroscopeSection,
    inputGallery,
  },
  props: {
    id: String,
  },
  data: () => ({
    selectedSection: 0,
    newDocuments: [],
    indexChoose: 0,
  }),
  computed: {
    ...mapState({
      telescopeDto: (state) => state.telescope.telescopeDto,
      subjectsList: (state) => state.subjects.subjectsList,
      documentIndex: (state) => state.telescope.documentIndex,
      typeListDescription: (state) => state.telescope.typeListDescription,
      selectedSectionDto: (state) => state.telescope.selectedSectionDto,
    }),
    ...mapGetters(["years", "tagsList", "doctors"]),
  },
  created() {
    this.microscopeDetails(this.id);
    this.fetchSubject();
    this.fetchTotalTag();
  },
  methods: {
    ...mapActions([
      "updateMicroscope",
      "microscopeDetails",
      "fetchTotalTag",
      "fetchSubject",
      "deleteMicroscope",
      "updateSectionsMicroscope",
      "deleteSectionMicroscope"
    ]),
    ...mapMutations(["UPDATE_DOCUMENT_INDEX", "UPDATE_TYPE_LIST_DESCRIPTION","UPDA"]),
    onSubmit() {
      this.$refs.observertelescope.validate().then((success) => {
        if (success) {
          this.updateMicroscope({
            id: this.telescopeDto.id,
            name: this.telescopeDto.name,
            year: this.telescopeDto.year,
            type: this.telescopeDto.type,
            subjectId: this.telescopeDto.subjectId,
            price: this.telescopeDto.price,
            isFree: this.telescopeDto.isFree,
            tagIds: [...this.telescopeDto.doctors, ...this.telescopeDto.tags],
          });
        }
      });
    },
    onRemove() {
      this.deleteMicroscope(this.telescopeDto.id);
    },
   
    OnDeleteSectionMicroscope(id){
        this.deleteSectionMicroscope(id).then( () => {
          Object.assign(this.selectedSectionDto, {});
          this.selectedSection = 0;
        });
    },

    SubmitUpdateSectionMicroscope() {
      this.$refs.sectionMicroscope.validate().then((success) => {
        if (success) {
          var data = new FormData();
          console.log(this.selectedSectionDto.id);
          data.append("Id", this.id);
          data.append("Sections[0].Id", this.selectedSectionDto.id);
          data.append("Sections[0].Title", this.selectedSectionDto.title);
          data.append("Sections[0].Order", "1");
          data.append("Sections[0].QuestionType", "1");
          data.append("Sections[0].AnswerType", "1");

          var i = 0;
          console.log(this.selectedSectionDto.documents);
          this.selectedSectionDto.documents
            .filter((element) => !element.isDeleted)
            .forEach((element) => {
              console.log(element.id);
              data.append(
                `Sections[0].Documents[${i}].SectionImageId`,
                element.sectionImageId
              );
              data.append(`Sections[0].Documents[${i}].Note`, element.note);
              data.append(`Sections[0].Documents[${i}].Path`, element.path);
              data.append(`Sections[0].Documents[${i}].File`, element.file);
              i++;
            });
          if (this.newDocuments != null) {
            this.newDocuments.forEach((element) => {
              data.append(`Sections[0].Documents[${i}].Note`, element.note);
              data.append(`Sections[0].Documents[${i}].Path`, element.path);
              data.append(`Sections[0].Documents[${i}].File`, element.file);
              i++;
            });
          }

          console.log(data);
          this.updateSectionsMicroscope(data);
        }
      });
    },
    uploadImages(event) {
      this.newDocuments = event;
    },
    toggleImage(index) {
      this.selectedSectionDto.documents[index].isDeleted =
      !this.selectedSectionDto.documents[index].isDeleted;
      console.log(this.selectedSectionDto.documents[index].isDeleted)
    },
    chooseImage(index) {
      this.indexChoose = index;
      console.log(index);
      this.UPDATE_TYPE_LIST_DESCRIPTION(0);
    },
  },
  watch: {
    selectedSection(id) {
     // this.UPDATE_SELECTED_SECTION_DTO(id)
      Object.assign(this.selectedSectionDto , this.telescopeDto.sections.find(
        (sec) => sec.id == id
      ));
      this.selectedSectionDto.documents.forEach((element) => {
        Object.assign(element, { isDeleted: false });
      });
      console.log(this.selectedSectionDto.documents);
    },
  },
};
</script>
sty