<?php
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

require 'PHPMailer/src/Exception.php';
require 'PHPMailer/src/PHPMailer.php';
require 'PHPMailer/src/SMTP.php';

// Enable CORS for AJAX requests
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: POST');
header('Access-Control-Allow-Headers: Content-Type');

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Validate and sanitize input
    $name = isset($_POST['name']) ? htmlspecialchars(trim($_POST['name'])) : '';
    $email = isset($_POST['email']) ? filter_var(trim($_POST['email']), FILTER_SANITIZE_EMAIL) : '';
    $message = isset($_POST['message']) ? htmlspecialchars(trim($_POST['message'])) : '';

    // Basic validation
    if (empty($name) || empty($email) || empty($message)) {
        http_response_code(400);
        echo "All fields are required.";
        exit;
    }

    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        http_response_code(400);
        echo "Invalid email address.";
        exit;
    }

    $mail = new PHPMailer(true);

    try {
        // Server settings
        $mail->isSMTP();
        $mail->Host       = 'smtp.gmail.com';
        $mail->SMTPAuth   = true;
        
        // SECURITY NOTE: Move these to environment variables or config file
        // For example, create a config.php file that's not in your public directory
        $mail->Username   = 'datodrlee@gmail.com';
        $mail->Password   = 'decc sgrn zuuw dyxh';
        
        $mail->SMTPSecure = PHPMailer::ENCRYPTION_SMTPS;
        $mail->Port       = 465;

        // Recipients
        $mail->setFrom('datodrlee@gmail.com', 'Project Solaria'); 
        $mail->addAddress('aimsityinterns@gmail.com', 'Project Solaria Team');
        
        // Send copy to user
        $mail->addAddress($email, $name); 
        
        // Set reply-to as the original sender
        $mail->addReplyTo($email, $name);

        // Content
        $mail->isHTML(true);
        $mail->CharSet = 'UTF-8';

        // HTML Body Content
        $html_body = "
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset='UTF-8'>
            <style>
                body { font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px; margin: 0; }
                .container { max-width: 600px; margin: 0 auto; background-color: #fff; padding: 30px; border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
                .header { color: #333; text-align: center; margin-bottom: 20px; border-bottom: 2px solid #f1c40f; padding-bottom: 15px; }
                .content { color: #666; font-size: 16px; line-height: 1.6; }
                .details-table { width: 100%; border-collapse: collapse; margin: 20px 0; }
                .details-table td { padding: 12px; border: 1px solid #ddd; }
                .label { background-color: #f9f9f9; font-weight: bold; width: 150px; }
                .footer { margin-top: 30px; color: #888; font-size: 14px; text-align: center; border-top: 1px solid #eee; padding-top: 20px; }
            </style>
        </head>
        <body>
            <div class='container'>
                <h2 class='header'>New Contact Form Submission - Project Solaria</h2>
                <div class='content'>
                    <p>You have received a new message from your Project Solaria contact form:</p>
                    <table class='details-table'>
                        <tr>
                            <td class='label'>Name:</td>
                            <td>" . htmlspecialchars($name) . "</td>
                        </tr>
                        <tr>
                            <td class='label'>Email:</td>
                            <td>" . htmlspecialchars($email) . "</td>
                        </tr>
                        <tr>
                            <td class='label'>Message:</td>
                            <td>" . nl2br(htmlspecialchars($message)) . "</td>
                        </tr>
                        <tr>
                            <td class='label'>Submitted:</td>
                            <td>" . date('Y-m-d H:i:s') . "</td>
                        </tr>
                    </table>
                </div>
                <div class='footer'>
                    <p>This message was sent from the Project Solaria contact form.</p>
                </div>
            </div>
        </body>
        </html>";

        $mail->Subject = "Project Solaria - New Contact Form Submission from " . $name;
        $mail->Body    = $html_body;
        
        // Plain text version
        $mail->AltBody = "Project Solaria - New Contact Form Submission\n\n" .
                        "Name: " . $name . "\n" .
                        "Email: " . $email . "\n" .
                        "Submitted: " . date('Y-m-d H:i:s') . "\n\n" .
                        "Message:\n" . $message;

        $mail->send();
        
        // Log successful submission (optional)
        error_log("Contact form submission successful from: " . $email);
        
        http_response_code(200);
        echo "Message sent successfully! Thank you for contacting Project Solaria.";
        
    } catch (Exception $e) {
        // Log the error
        error_log("Contact form error: " . $mail->ErrorInfo);
        
        http_response_code(500);
        echo "Failed to send message. Please try again later or contact us directly at aimsitytrainers@gmail.com";
    }
} else {
    http_response_code(405);
    echo "Method not allowed. Please use the contact form.";
}
?>