<template>
    <div>
        <div v-if="isReadOnly">
            <b-card :title="book.title"
                    :img-src="book.thumbnailUrl"
                    img-alt="Image"
                    img-top
                    tag="article"
                    style="max-width: 30rem;"
                    class="mb-2">
                <b-card-text>
                    {{ book.descr }}
                </b-card-text>

                <b-form-checkbox switch v-model="isInEditMode" class="mb-2">Edit Book</b-form-checkbox>
                <b-button to="/" variant="secondary">Back</b-button>
            </b-card>
        </div>
        <div v-else>
            <b-card class="mb-2">
                <b-form label="Add/Edit Book Details" @submit="onSubmit" label-size="lg" label-class="font-weight-bold pt-0" class="mb-2">
                    <b-form-group label-cols-sm="3" label="Title:" label-for="nested-title">
                        <b-form-input id="nested-title" v-model="book.title" placeholder="Enter book title" :state="titleState"></b-form-input>
                    </b-form-group>
                    <b-form-group label-cols-sm="3" label="Thumbnail Url:" label-for="nested-thumbnailUrl">
                        <b-form-input id="nested-thumbnailUrl" v-model="book.thumbnailUrl" placeholder="Enter book thumbnail url"  :state="urlState"></b-form-input>
                    </b-form-group>
                    <b-form-group label-cols-sm="3" label="Decription:" label-for="nested-descr">
                        <b-form-textarea id="nested-descr" v-model="book.descr" placeholder="Enter book description"  :state="descriptionState"></b-form-textarea>
                    </b-form-group>
                    <b-form-group label-cols-sm="3" label="ISBN:" label-for="nested-isbn">
                        <b-form-input id="nested-isbn" v-model="book.isbn" placeholder="Enter book ISBN url"></b-form-input>
                    </b-form-group>
                    <b-form-group label-cols-sm="3" label="Published Date:" label-for="nested-publisheddate">
                        <b-form-input id="nested-publisheddate" v-model="book.publishedDate" class="mb-2" type="date" :state="dateState">  </b-form-input>
                    </b-form-group>
                    <b-button type="submit" variant="primary">Submit</b-button>
                    <b-button to="/" variant="secondary" class="mx-2">Back</b-button>
                </b-form>
            </b-card>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    //import router from '../routes';

    export default {
        name: 'Book',
        props: ["id"],
        data: () => ({
            isInEditMode: false,
            book: null
        }),

        computed: {
            isReadOnly() {
                return this.book && !this.isInEditMode;
            },
            titleState() {
                return this.book.title.length > 2;
            },
            descriptionState() {
                return this.book.descr.length > 2;
            },
            urlState() {
                return this.book.thumbnailUrl.length > 2;
            },
            dateState() {
                try {
                    var date = Date.parse(this.book.publishedDate);
                    return date > 0;
                }
                catch{
                    return false;
                }
            }
        },
        methods: {
            onSubmit() {
                axios.post(`https://localhost:5001/books`, this.book)
                    .then(response => {
                        this.book = response.data;
                        this.isInEditMode = false;
                    });
            },
        },
        created() {            
            this.book = {
                bookId: 0,
                title: '',
                isbn: '',
                publishedDate: '',
                thumbnailUrl: '',
                descr: ''
            };
            if (this.id === undefined) {
                this.isInEditMode = true;
            }
        },
        mounted() {
            if (this.id > 0) {
                axios.get(`https://localhost:5001/books/${this.id}`)
                    .then(response => {
                        this.book = response.data;
                    });
            }
        }
    }
</script>