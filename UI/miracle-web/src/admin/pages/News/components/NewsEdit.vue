<template>
  <ValidationObserver ref="observer">
    <v-card class="pa-10 pt-5" elevation="5">
      <a @click="close(false)"><v-icon style="float: right;">mdi-close-circle</v-icon></a>
      <h2 class="text-md-center">{{ translator("editNews") }}</h2>
      <hr class="mb-2" />

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
            <img id="vimg" :src="newsImage" style="display: inline-block; background-size: contain; width: 100%;" />
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

        <v-btn @click.prevent="newsEdit" color="primary" dark class="v-btn--block v-size--large mb-2">
          {{ translator("update") }}
        </v-btn>
      </v-form>
    </v-card>

    <v-dialog v-model="dialogEnable" max-width="450" persistent :key="dialogKey">
      <component :is="selectedComponent" v-bind="componentProps" @closed="closeDialog" />
    </v-dialog>
  </ValidationObserver>
</template>

<script>
import { extend, ValidationObserver, ValidationProvider, setInteractionMode } from "vee-validate";
import { Base } from "../../../../helpers/RouteConstraints";
import * as rules from "vee-validate/dist/rules";
import MessageBox from "../../../../helpers/components/MessageBox";
import NewsService from "../../../../services/NewsService";
import { ReturnConstraints } from "@/helpers/Constraints";

setInteractionMode("eager");

extend("required", { ...rules.required, message: "{_field_} can not be empty" });

export default {
  props: ["newsIdProp"],
  data() {
    return {
      news: {
        startDate: new Date().toISOString().substr(0, 10),
        endDate: new Date().toISOString().substr(0, 10),
      },
      endDateModal: false,
      startDateModal: false,
      newsId: null,
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
    async initialize() {
      let newsResponse = await new NewsService().getById(this.newsId);
      if (newsResponse === ReturnConstraints) await this.initialize();

      this.news.id = this.newsId;

      this.news = newsResponse.data;
      this.news.startDate = newsResponse.data.startDate.toString().substr(0, 10);
      this.news.endDate = newsResponse.data.endDate.toString().substr(0, 10);

      let imageResponse = await new NewsService().getImage(this.newsId);
      const img = document.getElementById("vimg");
      img.src = Base + imageResponse;
    },
    async newsEdit() {
      let result = await this.$refs.observer.validate();
      if (result) {
        let formData = new FormData();

        for (let [key, value] of Object.entries(this.news)) {
          formData.append(`${key}`, `${value}`);
        }
        formData.append("newsImage", this.news.newsImage);
        let response = await new NewsService().edit(formData, this.newsId);
        if (response.success) this.close(true);
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
    this.newsId = this.newsIdProp;
    this.editorConfig.language = this.$vuetify.lang.current;
    this.initialize();
  },
};
</script>

<style scoped>
.rows {
  margin-bottom: -4%;
}

#rows1 {
  margin-bottom: -3%;
}
</style>
