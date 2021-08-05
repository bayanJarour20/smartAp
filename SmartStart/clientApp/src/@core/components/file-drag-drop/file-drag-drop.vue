<template>
<div>
    <label
        :for="id + 'input-' + (mul ? 'mul' : 'single')"
        class="drop border-primary"
        :style="'background:url(' + placehoder + ') center center no-repeat; background-size: cover; height:' + height">
        <div 
            :class="{'over': isDragging, 'error': wrongFile}"
            @dragover.prevent="dragOver" 
            @dragleave.prevent="dragLeave"
            @drop.prevent="drop($event)"
        >
            <h5 class="text-primary m-0">{{ title }}
                <unicon fill="#7367f0" width="20" name="plus"></unicon>
            </h5>
            <input v-if="mul" type="file" multiple :id="id + 'input-mul'" @change="drop($event)" class="d-none" >
            <input v-else type="file" :id="id + 'input-single'" @change="drop($event)" class="d-none" >
        </div>
    </label>
    <small>{{progress}}</small>
</div>
</template>
<style lang="scss">
    .drop {
        cursor: pointer;
        transition: all .4s ease-in-out;
        width: 100%;
        display: flex;
        background: #efefef;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        padding: 6px;
        margin: 0;
        border-style: dashed!important;
    }
    .over {
        background: #85e783;
   }
</style>
<script>
import ToastificationContent from "@core/components/toastification/ToastificationContent.vue";

export default {
    name: "file-drag-drop",
    data: () => ({
        isDragging: false,
        wrongFile: false
    }),
    props: {
        title: String,
        mul: Boolean,
        id: String,
        placehoder: String,
        progress: String,
        type: {
            type: String,
            required: true
        },
        height: String
    },
    methods: {
        dragOver(){
            this.isDragging = true;
        },
        dragLeave(){
            this.isDragging = false;
        },
        drop(e){
            let files = e.dataTransfer == null ? e.target.files : e.dataTransfer.files;
            this.wrongFile = false;
            this.isDragging = false;
            // allows only 1 file
            if(files.length === 0){
                return
            } else if (files.length === 1 && !this.mul) {
                let file = files[0];
                console.log(file.type, this.type, file.type.indexOf(this.type))
                if (file.type.indexOf(this.type) != -1) {
                    this.$emit("uploaded", file)
                } else {
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
                    }, 300)
                }
            } else {
                let file = files;
                file.forEach((file) => {
                    if (file.type.indexOf('image/') < 0) {
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
                    }
                })
                this.$emit("uploaded", file);
            }
        },
    }
}
</script>