variable "aws_region" {
  default = "eu-north-1"
}

variable "instance_type" {
  default = "t3.micro"
}

variable "ssh_key_name" {
  description = "Name of SSH key in AWS"
}

variable "cloudflare_api_token" {
  sensitive = true
}

variable "cloudflare_zone_id" {
  description = "Zone ID from Cloudflare dashboard"
}

variable "domain_name" {
  description = "e.g. app"
}

variable "state_passphrase" {
  description = "Password to encrypt the state file"
  sensitive = true
}

variable "ami" {
  default = "ami-073130f74f5ffb161"
}
