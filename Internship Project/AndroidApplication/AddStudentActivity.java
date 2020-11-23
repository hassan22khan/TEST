package com.example.internship;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Base64;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.util.ArrayList;
import java.util.List;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class AddStudentActivity extends AppCompatActivity {
    private List<String> selectedCourseIds = null;
    private ImageView imageView;
    private Bitmap selectedImageBitmap;
    private String userId;
    private String token;
    private int id;
    OkHttpClient okHttpClient = UnsafeOkHttpClient.getUnsafeOkHttpClient();
    Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("https://192.168.1.8:45455/")
            .client(okHttpClient)
            .addConverterFactory(GsonConverterFactory.create())
            .build();
    StudentApi studentApi = retrofit.create(StudentApi.class);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_student);

        imageView = (ImageView) findViewById(R.id.imageView);
        Button selectCoursesButton = findViewById(R.id.selectCoursesButton);

        selectCoursesButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getCoursesCall();
            }
        });

        Button chooseImageButton = findViewById(R.id.chooseImageButton);
        chooseImageButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                selectImage(AddStudentActivity.this);
            }
        });

        Button submitButton = findViewById(R.id.submitButton);
        submitButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Student student = new Student();
                String[] imageFileArray = new String[2];
                TextView username = findViewById(R.id.usernameTextView);
                student.name = username.getText().toString();
                TextView email = findViewById(R.id.emailTextView);
                student.email= email.getText().toString();
                TextView dob = findViewById(R.id.dobTextView);
                student.dob = dob.getText().toString();
                TextView phone = findViewById(R.id.phoneTextView);
                student.phone= phone.getText().toString();
                TextView password = findViewById(R.id.passwordTextView);
                student.password = password.getText().toString();
                TextView confirmPassword = findViewById(R.id.confirmPasswordTextView);
                student.confirmPassword = confirmPassword.getText().toString();
                userId = getIntent().getStringExtra("userId");
                token = getIntent().getStringExtra("token");
                id = getIntent().getIntExtra("id",0);
                student.userId = Integer.parseInt(userId);

                //Convert bitmap to Image
                if(selectedImageBitmap != null){
                    ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                    selectedImageBitmap.compress(Bitmap.CompressFormat.PNG, 100, byteArrayOutputStream);
                    byte[] byteArray = byteArrayOutputStream .toByteArray();

                    String imageBase64 = Base64.encodeToString(byteArray, Base64.DEFAULT);
                    imageFileArray  = imageBase64.split(",", 2);
                }
                StudentDto dtoObject = new StudentDto();
                dtoObject.student = student;
                dtoObject.courses = selectedCourseIds;
                dtoObject.imageFile = imageFileArray;
                //  postStudent();
                postStudent(dtoObject,token);
            }
        });
    }

    private void getCoursesCall(){
        Call<List<Course>> call = studentApi.getCourses();
        call.enqueue(new Callback<List<Course>>() {
            @Override
            public void onResponse(Call<List<Course>> call, Response<List<Course>> response) {
                Log.e("Response",response.message());
                List<Course> courses = response.body();
                selectCoursesDialoge(courses);
            }

            @Override
            public void onFailure(Call<List<Course>> call, Throwable t) {
                Log.e("Error", t.getMessage());
            }
        });
    }

    private void postStudent(StudentDto dtoObject,String token){
        if(id == 0){
            Call<String> call = studentApi.postStudent(dtoObject, "bearer " + token);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                   redirectToStudentListActivity();
                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("Error", t.getMessage());
                }
            });
        }else{
            dtoObject.student.id = id;
            Call<String> call = studentApi.editStudent(dtoObject,"bearer " + token);
            call.enqueue(new Callback<String>() {
                @Override
                public void onResponse(Call<String> call, Response<String> response) {
                   redirectToStudentListActivity();
                }

                @Override
                public void onFailure(Call<String> call, Throwable t) {
                    Log.e("Error", t.getMessage());
                }
            });
        }
    }

    private void redirectToStudentListActivity(){
        Intent intent = new Intent(AddStudentActivity.this, StudentListActivity.class);
        intent.putExtra("userId", userId);
        intent.putExtra("token", token);
        startActivity(intent);
    }

    private void selectImage(Context context) {
        final CharSequence[] options = { "Take Photo", "Choose from Gallery","Cancel" };

        AlertDialog.Builder builder = new AlertDialog.Builder(context);
        builder.setTitle("Choose your profile picture");

        builder.setItems(options, new DialogInterface.OnClickListener() {

            @Override
            public void onClick(DialogInterface dialog, int item) {

                if (options[item].equals("Take Photo")) {
                    Intent takePicture = new Intent(android.provider.MediaStore.ACTION_IMAGE_CAPTURE);
                    startActivityForResult(takePicture, 0);

                } else if (options[item].equals("Choose from Gallery")) {
                    Intent pickPhoto = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
                    startActivityForResult(pickPhoto , 1);

                } else if (options[item].equals("Cancel")) {
                    dialog.dismiss();
                }
            }
        });
        builder.show();
    }

    public void selectCoursesDialoge(List<Course> courses){
        String[] coursesNames =  new String[courses.size()];
        int[] courseIds =  new int[courses.size()];
        selectedCourseIds= new ArrayList<String>();
        for(int i=0; i< courses.size(); i++){
            coursesNames[i] = courses.get(i).name;
            courseIds[i] = courses.get(i).id;
        }
        AlertDialog.Builder builder = new AlertDialog.Builder(AddStudentActivity.this);
        builder.setTitle(" Courses ");
        builder.setMultiChoiceItems(coursesNames, null, new DialogInterface.OnMultiChoiceClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which, boolean isChecked) {
                if (isChecked) {
                    selectedCourseIds.add(Integer.toString(courseIds[which]));
                } else if (selectedCourseIds.contains(Integer.toString(courseIds[which]))) {
                    selectedCourseIds.remove(Integer.valueOf(which));
                }
            }})
                .setCancelable(false)
                .setPositiveButton("Yes", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id)
                    {
                    }
                })
                .setNegativeButton("No", new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int id) {
                        selectedCourseIds = null;
                        dialog.cancel();
                    }
                }).show();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (resultCode != RESULT_CANCELED) {
            switch (requestCode) {
                case 0:
                    if (resultCode == RESULT_OK && data != null) {
                        selectedImageBitmap = (Bitmap) data.getExtras().get("data");
                        imageView.setImageBitmap(selectedImageBitmap);
                    }

                    break;
                case 1:
                    if (resultCode == RESULT_OK && data != null) {
                        Uri selectedImage = data.getData();
                        String[] filePathColumn = {MediaStore.Images.Media.DATA};
                        if (selectedImage != null) {
                            Cursor cursor = getContentResolver().query(selectedImage,
                                    filePathColumn, null, null, null);
                            if (cursor != null) {
                                cursor.moveToFirst();

                                int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                                String picturePath = cursor.getString(columnIndex);
                                selectedImageBitmap = BitmapFactory.decodeFile(picturePath);
                                imageView.setImageBitmap(selectedImageBitmap);
                                cursor.close();
                            }
                        }

                    }
                    break;
            }
        }
    }

}