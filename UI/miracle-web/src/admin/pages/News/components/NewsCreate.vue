<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)">
        <v-icon style="float: right;">mdi-close-circle</v-icon>
      </a>
      <h2 class="text-md-center">{{ translator("newNews") }}</h2>
      <hr class="mb-3" />

      <v-form>
        <v-row id="rows1">
          <v-col cols="12" md="5">
            <v-file-input
              prepend-inner-icon="mdi-camera"
              accept="image/*"
              :placeholder="translator('newsImage')"
              @change="uploadImage"
            ></v-file-input>
          </v-col>
          <v-col cols="12" md="7">
            <img
              id="vimg"
              :src="newsImage"
              style="display: inline-block; background-size: contain; width: 100%;"
              alt=""
            />
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('title')" rules="required|max:128">
              <v-text-field
                v-model="news.title"
                :error-messages="errors"
                :label="translator('title')"
                required
                outlined
                counter="128"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('tags')" rules="required|max:32">
              <v-text-field
                v-model="news.tags"
                :error-messages="errors"
                :label="translator('tags')"
                required
                outlined
                counter="32"
              ></v-text-field>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="12">
            <ValidationProvider #default="{ errors }" :name="translator('text')" rules="required">
              <ckeditor v-model="news.text" :config="editorConfig" :error-messages="errors"></ckeditor>
            </ValidationProvider>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <v-dialog
              ref="sDialog"
              v-model="startDateModal"
              :return-value.sync="news.startDate"
              width="290px"
              persistent
            >
              <template #activator="{ on }">
                <v-text-field
                  v-model="news.startDate"
                  prepend-inner-icon="mdi-calendar"
                  v-on="on"
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="news.startDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="startDateModal = false">Cancel</v-btn>
                <v-btn text color="primary" @click="$refs.sDialog.save(news.startDate)">OK</v-btn>
              </v-date-picker>
            </v-dialog>
          </v-col>
          <v-col cols="12" md="6">
            <v-dialog ref="eDialog" v-model="endDateModal" :return-value.sync="news.endDate" persistent width="290px">
              <template #activator="{ on }">
                <v-text-field
                  v-model="news.endDate"
                  prepend-inner-icon="mdi-calendar"
                  v-on="on"
                  outlined
                ></v-text-field>
              </template>
              <v-date-picker v-model="news.endDate" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="primary" @click="endDateModal = false">Cancel</v-btn>
                <v-btn text color="primary" @click="$refs.eDialog.save(news.endDate)">OK</v-btn>
              </v-date-picker>
            </v-dialog>
          </v-col>
        </v-row>

        <v-row class="rows">
          <v-col cols="12" md="6">
            <v-switch v-model="news.isActive" :label="translator('isActive')" required outlined></v-switch>
          </v-col>
        </v-row>

        <hr class="mb-2" />

        <v-btn @click.prevent="newsCreate" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("create") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="400" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import * as rules from "vee-validate/dist/rules";
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import MessageBox from "../../../../helpers/components/MessageBox";
import NewsService from "../../../../services/NewsService";

setInteractionMode("eager");
extend("required", { ...rules.required, message: "{_field_} can not be empty" });
extend("max", { ...rules.max, message: "{_field_} may not be greater than {length} characters" });

export default {
  data() {
    return {
      startDateModal: false,
      endDateModal: false,
      news: {
        startDate: new Date().toISOString().substr(0, 10),
        endDate: new Date().toISOString().substr(0, 10),
      },
      newsImage: "/miracle-logo.png",
      dialog: false,
      signalModel: {
        changes: false,
        returnValues: null,
      },

      dialogEnable: false,
      dialogKey: 0,
      selectedComponent: "",
      componentProps: null,

      editorConfig: {
        extraPlugins: ["justify"],
        toolbar: [
          { name: "document", items: ["Source"] },
          { name: "clipboard", items: ["Undo", "Redo"] },
          { name: "editing", items: ["Find", "Replace", "-", "SelectAll", "-"] },
          { name: "basicstyles", items: ["Bold", "Italic", "Underline", "-", "CopyFormatting", "RemoveFormat"] },
          {
            name: "paragraph",
            items: [
              "NumberedList",
              "BulletedList",
              "-",
              "Outdent",
              "Indent",
              "-",
              "Blockquote",
              "-",
              "JustifyLeft",
              "JustifyCenter",
              "JustifyRight",
              "JustifyBlock",
              "-",
              "BidiLtr",
              "BidiRtl",
            ],
          },
          { name: "links", items: ["Link", "Unlink"] },
          { name: "insert", items: ["Image", "Table"] },
          { name: "styles", items: ["Styles", "Format", "Font", "FontSize"] },
          { name: "colors", items: ["TextColor", "BGColor"] },
        ],
      },
    };
  },
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  methods: {
    async newsCreate() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();
        for (let [key, value] of Object.entries(this.news)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("newsImage", this.news.newsImage);
        let response = await new NewsService().create(formData);
        if (response.success) this.close("closed", true);
        else {
          this.selectedComponent = MessageBox;
          this.componentProps = {
            messageTitleProp: this.translator("failed"),
            messageTextProp: response.message,
          };
          this.dialogEnable = true;
          this.dialogKey += 1;
        }
      }
    },
    uploadImage(image) {
      this.news.newsImage = image;
      let reader = new FileReader();
      reader.onload = function (e) {
        document.getElementById("vimg").setAttribute("src", e.target.result.toString());
      };
      reader.readAsDataURL(image);
    },
    closeDialog() {
      this.dialogEnable = false;
      this.dialogKey += 1;
    },
    close(changes) {
      this.signalModel.changes = changes;
      this.$emit("closed", this.signalModel);
    },
  },
  created() {
    this.editorConfig.language = this.$vuetify.lang.current;
    this.news.isActive = false;
  },
};
</script>

<style scoped>
.rows {
  margin-bottom: -5%;
}

#rows1 {
  margin-bottom: -3%;
}
</style>
