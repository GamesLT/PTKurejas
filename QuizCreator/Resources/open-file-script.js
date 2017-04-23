function ptkurejas_quiz(data) {
    return data;
}

jsText

for (i = 0; i < quiz_photo_data.answers.length; i++) {
    if (typeof quiz_photo_data.images[i] != 'undefined') {
        window.external.AddImage(quiz_photo_data.answers[i], quiz_photo_data.images[i]);
    } else {
        window.external.AddAnswer(quiz_photo_data.answers[i]);        
    }
}
window.external.Finish();