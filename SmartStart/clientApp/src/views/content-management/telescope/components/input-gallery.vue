<template>
    <b-row>
        <b-col cols="3">
            <div class="position-relative mb-1">
                <b-button
                    size="sm"
                    class="clear btn-icon border-0"
                    variant="flat-secondary"
                    @click="clear"
                >
                    <unicon
                        name="trash-alt"
                        fill="#ea5455"
                        width="16"
                        height="16"
                    />
                </b-button>
                <label
                    :for="'label-for-file-' + id + (mul ? '-mul' : '-single')"
                    >{{ label }}</label
                >
                <label
                    :for="'label-for-file-' + id + (mul ? '-mul' : '-single')"
                    class="w-100"
                >
                    <div
                        :class="{ over: isDragging, error: wrongFile }"
                        class="
              drop-container
              rounded
              overflow-hidden
              w-100
              d-flex
              justify-content-center
              align-items-center
            "
                        style="height: 150px; box-shadow: 0px 0px 5px 0px #0000003d"
                        @dragover.prevent="dragOver"
                        @dragleave.prevent="dragLeave"
                        @drop.prevent="drop($event)"
                    >
                        <h5 class="text-primary m-0">
                            {{ title }}
                            <unicon
                                fill="#7367f0"
                                width="20"
                                name="plus"
                            ></unicon>
                        </h5>
                        <input
                            v-if="mul"
                            v-bind="$attrs"
                            type="file"
                            multiple
                            :id="'label-for-file-' + id + '-mul'"
                            @change="drop($event)"
                            class="d-none"
                        />
                        <input
                            v-else
                            type="file"
                            v-bind="$attrs"
                            :id="'label-for-file-' + id + '-single'"
                            @change="drop($event)"
                            class="d-none"
                        />
                    </div>
                </label>

                <small
                    class="text-danger"
                    v-if="
                        required &&
                            ((mul &&
                                (!previewBase ||
                                    (previewBase && !previewBase.length))) ||
                                (!mul && !previewBase))
                    "
                    >{{ name ? name : "الصورة" }} مطلوبة</small
                >
                <small>{{ progress }}</small>
            </div>
        </b-col>
        <b-col cols="3" v-if="previewBase && !mul">
            <img class="image" :src="previewBase" />
        </b-col>
        <b-col cols="3" v-for="(src, index) in previewBase" :key="index">
            <div
                class="
          drop-container
          rounded
          overflow-hidden
          d-flex
          justify-content-center
          align-items-center
        "
                @click="chooseImage(index)"
                style="height: 150px; box-shadow: 0px 0px 5px 0px #0000003d; cursor: pointer;"
                :style="
                    isChoose == index && typeListDescription == 1
                        ? 'border: #6610f2; border-style: solid;'
                        : ''
                "
            >
                <img
                    class="image multie-image"
                    :src="src"
                    style=""
                    width="100%"
                />
            </div>
        </b-col>
    </b-row>
</template>
<script>
import ToastificationContent from "@core/components/toastification/ToastificationContent.vue";
import { BButton } from "bootstrap-vue";
import { mapState, mapMutations } from "vuex";
export default {
    computed: {
        ...mapState({
            documentIndex: state => state.telescope.documentIndex,
            typeListDescription: state => state.telescope.typeListDescription
        })
    },
    components: {
        BButton
    },
    data: () => ({
        innerVal: null,
        previewBase: null,
        id: Math.random() * 100000,
        isDragging: false,
        wrongFile: false,
        isChoose: -1
    }),
    props: {
        title: String,
        mul: Boolean,
        name: String,
        placehoder: String,
        progress: String,
        base64: Boolean,
        required: Boolean,
        label: String,
        height: String,
        value: {
            type: null
        }
    },
    mounted() {
        if (this.value) {
            this.previewBase = this.value;
        }
    },
    methods: {
        ...mapMutations([
            "UPDATE_DOCUMENT_INDEX",
            "UPDATE_TYPE_LIST_DESCRIPTION"
        ]),
        clear() {
            this.innerVal = null;
            this.previewBase = null;
        },
        dragOver() {
            this.isDragging = true;
        },
        dragLeave() {
            this.isDragging = false;
        },
        async drop(e) {
            let files =
                e.dataTransfer == null ? e.target.files : e.dataTransfer.files;
            this.wrongFile = false;
            this.isDragging = false;
            // allows only 1 file
            if (files.length === 0) {
                return;
            } else if (!this.mul) {
                if (files[0].type.indexOf("image") == -1) {
                    this.wrongFile = true;
                    this.$emit("inValidFile");
                    this.$toast({
                        component: ToastificationContent,
                        position: "top-right",
                        props: {
                            title: "نوع الملف غير مدعوم",
                            icon: "CoffeeIcon",
                            variant: "danger"
                        }
                    });
                    setTimeout(() => {
                        this.wrongFile = false;
                    }, 300);
                } else {
                    let file = files[0];
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = (() => {
                        this.previewBase = reader.result;
                        if (this.base64) {
                            this.innerVal = reader.result;
                        } else {
                            this.innerVal = file;
                        }
                    }).bind(this);
                }
            } else {
                let file = files;
                let acceptedFiles = [];
                this.previewBase = [];
                await file.forEach(file => {
                    if (file.type.indexOf("image") == -1) {
                        this.wrongFile = true;
                        this.$emit("inValidFile");
                        this.$toast({
                            component: ToastificationContent,
                            position: "top-right",
                            props: {
                                title: "نوع الملف " + file.name + "غير مدعوم",
                                icon: "CoffeeIcon",
                                variant: "danger"
                            }
                        });
                        setTimeout(() => {
                            this.wrongFile = false;
                        }, 300);
                    } else {
                        const reader = new FileReader();
                        reader.readAsDataURL(file);
                        reader.onload = (() => {
                            this.previewBase.push(reader.result);
                            if (this.base64) {
                                acceptedFiles.push(reader.result);
                            } else {
                                acceptedFiles.push(file);
                            }
                        }).bind(this);
                    }
                });
                this.innerVal = acceptedFiles;
            }
        },
        chooseImage(index) {
            this.UPDATE_DOCUMENT_INDEX(index);
            this.UPDATE_TYPE_LIST_DESCRIPTION(1);
            this.isChoose = index;
        }
    },
    watch: {
        innerVal(v) {
            var documents = [];
            if (v != null) {
                v.forEach(element =>
                    documents.push({ file: element, note: "", path: "" })
                );
            }
            this.$emit("input", documents);
        },
        value(v) {
            this.previewBase = v;
        }
    }
};
</script>
<style lang="scss">
.clear {
    position: absolute;
    top: -6px;
    right: 0;
}
.drop {
    cursor: pointer;
    transition: all 0.4s ease-in-out;
    width: 100%;
    display: flex;
    background: #efefef;
    justify-content: center;
    align-items: center;
    flex-direction: column;
    padding: 6px;
    margin: 0;
    border-style: dashed !important;
    border-bottom: none !important;
}
.over {
    background: #85e783;
}
.drop-container {
    width: 100%;
    text-align: center;
}
.preview {
    border-top: none !important;
    .image {
        width: 100%;
        max-height: 300px;
        object-fit: cover;
        &.multie-image {
            max-height: 200px;
            &:not(:last-of-type) {
                margin-bottom: 1rem;
            }
        }
    }
}
</style>
