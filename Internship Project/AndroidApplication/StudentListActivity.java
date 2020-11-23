package com.example.internship;

import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.core.widget.NestedScrollView;

import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;
import android.widget.Toast;

import java.util.List;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class StudentListActivity extends AppCompatActivity {

    LinearLayout linearLayout = null;
    String token = null;
    String userId = null;
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
        setContentView(R.layout.activity_student_list);
        userId = getIntent().getStringExtra("userId");
        token = getIntent().getStringExtra("token");

        Button addButton = findViewById(R.id.addButton);
        addButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                redirectToAddStudentActivity();
            }
        });

        getStudentsCall();
    }

    public void redirectToAddStudentActivity(){
        Intent intent = new Intent(StudentListActivity.this,AddStudentActivity.class);
        intent.putExtra("userId",userId);
        intent.putExtra("token",token);
        intent.putExtra("id",0);
        startActivity(intent);
    }

    public void getStudentsCall(){
        Call<List<StudentDto>> call = studentApi.getStudentDtos(userId,"bearer " + token);
        call.enqueue(new Callback<List<StudentDto>>() {
            @Override
            public void onResponse(Call<List<StudentDto>> call, retrofit2.Response<List<StudentDto>> response) {
                Log.e("Response",response.message());
                LinearLayout linearLayout = findViewById(R.id.linearLayout);
                List<StudentDto> studentDtoList = response.body();
                for (StudentDto student : studentDtoList) {
                    String content = "";
                    content += "Student Name: \t" + student.student.name+"\n";
                    content += "Student Email: \t" + student.student.email+ "\n";
                    content += "Student DOB: \t" + student.student.dob+ "\n";
                    content += "Student Password: \t" + student.student.password+ "\n";
                    content += "Student ConfirmPassword: \t" + student.student.confirmPassword+ "\n";
                    content += "Student PhoneNo: \t" + student.student.phone+ "\n";
                    TextView studentTextView = new TextView(StudentListActivity.this);
                    LinearLayout.LayoutParams lparams = new LinearLayout.LayoutParams(
                            LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);

                    studentTextView.setLayoutParams(lparams);
                    studentTextView.setText(content);
                    linearLayout.addView(studentTextView);
                    addButton("Edit",linearLayout,lparams, student.student.id);
                    addButton("Delete",linearLayout,lparams,student.student.id);
                }
            }

            @Override
            public void onFailure(Call<List<StudentDto>> call, Throwable t) {
                Log.e("Error",t.getMessage());
            }
        });
    }

    public void delete(int id){
        Call<String> call = studentApi.deleteStudent(id,"bearer " + token);
        call.enqueue(new Callback<String>() {
            @Override
            public void onResponse(Call<String> call, Response<String> response) {
                Log.e("Response", response.message());
                Intent intent = new Intent(StudentListActivity.this,StudentListActivity.class);
                intent.putExtra("userId",userId);
                intent.putExtra("token",token);
                startActivity(intent);
            }

            @Override
            public void onFailure(Call<String> call, Throwable t) {
                Log.e("Error", t.getMessage());
            }
        });
    }

    public void edit(int id){
        Intent intent = new Intent(StudentListActivity.this,AddStudentActivity.class);
        intent.putExtra("userId",userId);
        intent.putExtra("token",token);
        intent.putExtra("id",id);
        startActivity(intent);
    }

    public void addButton(String dataToDisplay, LinearLayout linearLayout, LinearLayout.LayoutParams lparams, int studentId){
        Button button = new Button(StudentListActivity.this);
        button.setText(dataToDisplay);
        button.setTextColor(Color.BLACK);

        if(dataToDisplay == "Delete"){
            button.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    delete(studentId);
                }
            });
        }else{
            button.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    edit(studentId);
                }
            });
        }

        linearLayout.addView(button);
    }
}