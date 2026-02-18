output "server_ip" {
  value = aws_instance.app_server.public_ip
}

output "domain_url" {
  value = "https://${var.domain_name}.pabu.dev"
}
